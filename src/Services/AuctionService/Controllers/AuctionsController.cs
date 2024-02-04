using AuctionService.Data;
using AuctionService.Dtos;
using AuctionService.Entities;
using IntegrationEvents.Events;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public AuctionsController(AuctionDbContext dbContext, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        var items = await _dbContext.Auctions.Include(a => a.Item).ToListAsync(cancellationToken);
        return Ok(_mapper.Map<List<Auction>, List<AuctionDto>>(items));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuctionDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _dbContext.Auctions.Include(a => a.Item)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if(item is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<Auction, AuctionDto>(item));
    }

    [Authorize]
    [RequiredScope("auctions.write")]
    [HttpPost]
    public async Task<ActionResult<AuctionDto>> Create([FromBody] CreateAuctionDto auctionDto, CancellationToken cancellationToken = default)
    {
        if(User.Identity?.Name is null)
        {
            return BadRequest();
        }

        var item = new Auction()
        {
            AuctionEnd = auctionDto.AuctionEnd,
            ReservePrice = auctionDto.ReservePrice ?? 0,
            SellerName = User.Identity.Name,
            Item = new AuctionItem()
            {
                Make = auctionDto.Make,
                Color = auctionDto.Color,
                ImageUrl = auctionDto.ImageUrl,
                Year = auctionDto.Year,
                Mileage = auctionDto.Mileage,
                Model = auctionDto.Model
            }
        };

         _dbContext.Add(item);
        await _publishEndpoint.Publish(_mapper.Map<Auction, AuctionCreatedEvent>(item));
        await _dbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new {item.Id}, _mapper.Map<Auction, AuctionDto>(item));
    }

    [Authorize]
    [RequiredScope("auctions.write")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid id,
     [FromBody] UpdateAuctionDto auctionDto,
     CancellationToken cancellationToken = default)
    {
        var item = await _dbContext.Auctions.Include(a => a.Item)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (item is null)
        {
            return NotFound();
        }

        if(User.Identity?.Name != item.SellerName)
        {
            return Forbid();
        }


        item.Item.Model = auctionDto.Model;
        item.Item.Make = auctionDto.Make;
        item.Item.Color = auctionDto.Color;
        item.Item.Year = auctionDto.Year;
        item.Item.Mileage = auctionDto.Mileage;

        await _publishEndpoint.Publish(_mapper.Map<Auction, AuctionUpdatedEvent>(item));
        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [Authorize]
    [RequiredScope("auctions.write")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id,
     CancellationToken cancellationToken = default)
    {
        var item = await _dbContext.Auctions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (item is null)
        {
            return NotFound();
        }

        if(User.Identity?.Name != item.SellerName)
        {
            return Forbid();
        }

        _dbContext.Remove(item);

        await _publishEndpoint.Publish(new AuctionDeletedEvent(item.Id));
        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
