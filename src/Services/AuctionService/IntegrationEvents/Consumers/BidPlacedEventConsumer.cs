using AuctionService.Data;
using AuctionService.Entities;
using AutoMapper;
using IntegrationEvents.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.IntegrationEvents.Consumers;

public class BidPlacedEventConsumer : IConsumer<BidPlacedEvent>
{
    private readonly AuctionDbContext _dbContext;
    private readonly ILogger<BidPlacedEventConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public BidPlacedEventConsumer(AuctionDbContext dbContext,
     ILogger<BidPlacedEventConsumer> logger,
     IPublishEndpoint publishEndpoint,
     IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<BidPlacedEvent> context)
    {
         _logger.LogInformation("----- Consuming {eventType}", typeof(BidPlacedEvent).Name);

        try
        {
            if (!Guid.TryParse(context.Message.AuctionId, out var auctionId))
            {
                throw new ArgumentException("Invalid auction id", nameof(BidPlacedEvent.AuctionId));
            }

            var auction = await _dbContext.Auctions.FirstOrDefaultAsync(x => x.Id == auctionId);

            if (auction is null)
            {
                _logger.LogWarning("Auction with the provided id = {id} is not found", auctionId);
                return;
            }

            if (context.Message.BidStatus == Enums.BidStatus.Accepted ||
                context.Message.BidStatus == Enums.BidStatus.AcceptedBelowReserve)
            {
                auction.CurrentHighBid = auction.CurrentHighBid.GetValueOrDefault(0) < context.Message.BidAmount ?
                     context.Message.BidAmount :
                     auction.CurrentHighBid;


                await _publishEndpoint.Publish(_mapper.Map<Auction, AuctionUpdatedEvent>(auction));
                await _dbContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            _logger.LogError("----- Error handling {eventType}. Error message: {errorMessage}",
                 typeof(BidPlacedEvent).Name,
                 e.Message);

            throw;
        }
    }
}