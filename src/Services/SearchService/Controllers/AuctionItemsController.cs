using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Dtos;
using SearchService.Entities;
using SearchService.Filtering;

namespace SearchService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionItemsController : ControllerBase
{
    private readonly IMapper _mapper;

    public AuctionItemsController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<FilteredResultsResponse<AuctionItemDto>>> Filter([FromQuery] AuctionItemsFilter filter, CancellationToken cancellationToken)
    {
        var query = DB.PagedSearch<AuctionItem, AuctionItem>();

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            query.Match(Search.Full, filter.SearchTerm).SortByTextScore();
        }

        query = filter.OrderBy?.ToLower() switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.Make))
                .Sort(x => x.Ascending(a => a.Model)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
        };

        query = filter.FilterBy?.ToLower() switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingsoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6)
                && x.AuctionEnd > DateTime.UtcNow),
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        };

        if (!string.IsNullOrEmpty(filter.Seller))
        {
            query.Match(x => x.SellerName == filter.Seller);
        }

        if (!string.IsNullOrEmpty(filter.Winner))
        {
            query.Match(x => x.Winner == filter.Winner);
        }

        query.PageNumber(filter.PageNumber);
        query.PageSize(filter.PageSize);

        var executionResult = await query.ExecuteAsync(cancellationToken);
        var resultDtos = _mapper.Map<IEnumerable<AuctionItem>, IEnumerable<AuctionItemDto>>(executionResult.Results);

        return Ok(new FilteredResultsResponse<AuctionItemDto>(resultDtos, executionResult.TotalCount, executionResult.PageCount));
    }
}