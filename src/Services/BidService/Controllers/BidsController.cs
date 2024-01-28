using AutoMapper;
using BidService.Data;
using BidService.Entities;
using BidService.Enums;
using BidService.Models;
using BidService.Services;
using IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BidService.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly BidDbContext _dbContext;
    private readonly ILogger<BidsController> _logger;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAuctionsGrpcService _auctionsGrpcService;


    public BidsController(BidDbContext dbContext,
        ILogger<BidsController> logger,
        IMapper mapper,
        IPublishEndpoint publishEndpoint,
        IAuctionsGrpcService auctionsGrpcService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _auctionsGrpcService = auctionsGrpcService;
    }

    [HttpGet]
    [Route("{id:long}")]
    public async Task<ActionResult<BidModel>> GetBidById([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        var bid = await _dbContext.Bids.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return bid is null ?
            NotFound(id) :
            Ok(_mapper.Map<Bid, BidModel>(bid));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BidModel>>> GetBidsByAuctionId([FromQuery] string auctionId, CancellationToken cancellationToken = default)
    {
        var bids = await _dbContext.Bids.Where(x => x.AuctionId == auctionId).OrderByDescending(x => x.BidTime).ToListAsync(cancellationToken);

        return Ok(_mapper.Map<IEnumerable<Bid>, IEnumerable<BidModel>>(bids));
    }

    [HttpPost]
    public async Task<ActionResult<BidModel>> PlaceBid([FromBody] PlaceBidRequestModel request, CancellationToken cancellationToken)
    {
        var auction = await _dbContext.Auctions.FirstOrDefaultAsync(x => x.Id == request.AuctionId, cancellationToken) ?? 
        await _auctionsGrpcService.GetAuctionByIdAsync(request.AuctionId, cancellationToken);

        if (auction is null)
        {
            _logger.LogError("Cannot find the auction with the provided id = {auctionId}", request.AuctionId);
            return NotFound();
        }

        if(auction.SellerName == User.Identity?.Name)
        {
            _logger.LogError("It is allowed to bid only on auctions created by other users");
            return BadRequest("It is allowed to bid only on auctions created by other users");
        }

        var bid = new Bid()
        {
            Amount = request.Amount,
            AuctionId = request.AuctionId,
            BidderName = User.Identity?.Name ?? "",
            BidTime = DateTime.UtcNow
        };

        if (auction.AuctionEnd <= DateTime.UtcNow)
        {
            bid.Status = BidStatus.Finished;
        }
        else
        {
            var highestBid = await _dbContext.Bids.OrderByDescending(x => x.Amount)
            .FirstOrDefaultAsync(x => x.AuctionId == request.AuctionId);

            if (highestBid is not null)
            {
                bid.Status = request.Amount > auction.ReservePrice && request.Amount > highestBid.Amount ?
                 BidStatus.Accepted :
                 auction.ReservePrice > request.Amount ? BidStatus.AcceptedBelowReserve : BidStatus.TooLow;

                highestBid.Status = request.Amount >= highestBid.Amount ? BidStatus.TooLow : highestBid.Status;
            }

            bid.Status = request.Amount > auction.ReservePrice ? BidStatus.Accepted : BidStatus.AcceptedBelowReserve;

            if(highestBid?.Status == BidStatus.TooLow)
            {
                await _publishEndpoint.Publish(new AcceptedBidStatusChangedEvent(highestBid.Id, request.AuctionId,
                     highestBid.BidderName, highestBid.BidTime, highestBid.Amount, Enum.GetName(highestBid.Status)!));
            }
        }

        _dbContext.Add(bid);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _publishEndpoint.Publish(_mapper.Map<Bid, BidPlacedEvent>(bid));

        return CreatedAtAction(nameof(GetBidById), new
        {
            id = bid.Id
        }, _mapper.Map<Bid, BidModel>(bid));
    }
}