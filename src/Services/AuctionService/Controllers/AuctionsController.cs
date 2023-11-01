using AuctionService.Data;
using AuctionService.Dtos;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _dbContext;
    private readonly IMapper _mapper;

    public AuctionsController(AuctionDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
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
        var item = await _dbContext.Auctions.Include(a => a.Item).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if(item is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<Auction, AuctionDto>(item));
    }

     [HttpPost]
    public async Task<ActionResult<AuctionDto>> Create([FromBody] CreateAuctionDto auctionDto, CancellationToken cancellationToken = default)
    {
        var item = new Auction()
        {
            AuctionEnd = auctionDto.AuctionEnd,
            ReservePrice = auctionDto.ReservePrice,
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
        await _dbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new {item.Id}, _mapper.Map<Auction, AuctionDto>(item));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid id,
     [FromBody] UpdateAuctionDto auctionDto,
     CancellationToken cancellationToken = default)
    {
        var item = await _dbContext.Auctions.Include(a => a.Item).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (item is null)
        {
            return NotFound();
        }

        item.Item.Model = auctionDto.Model;
        item.Item.Make = auctionDto.Make;
        item.Item.Color = auctionDto.Color;
        item.Item.Year = auctionDto.Year;
        item.Item.Mileage = auctionDto.Mileage;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id,
     CancellationToken cancellationToken = default)
    {
        var item = await _dbContext.Auctions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (item is null)
        {
            return NotFound();
        }

        _dbContext.Remove(item);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
