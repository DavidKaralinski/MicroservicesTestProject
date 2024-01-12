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
        var query = DB.PagedSearch<AuctionItem>();

        query.PageNumber(filter.PageNumber);
        query.PageSize(filter.PageSize);

        var entityProps = typeof(AuctionItem).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Select(x => x.Name);

        if(filter.OrderableFieldName is not null && entityProps.Any(x => x.Equals(filter.OrderableFieldName, StringComparison.InvariantCultureIgnoreCase)))
        {
             var fieldName = entityProps.First(x => x.Equals(filter.OrderableFieldName, StringComparison.InvariantCultureIgnoreCase));

             query.Sort(x => filter.OrderingDirection == Constants.OrderingDirection.Descending ?
                x.Descending(fieldName) :
                x.Ascending(fieldName));
        }
        else
        {
            query.Sort(x => x.Ascending(e => e.ReservePrice));
        }

        query.Match(x =>
            x.Year >= filter.YearFrom && x.Year <= filter.YearTo &&
            filter.EndingInLessThan == null ? 
                true : 
                x.AuctionEnd.HasValue && (DateTime.UtcNow - x.AuctionEnd.Value).TotalHours < filter.EndingInLessThan &&
            x.Mileage >= filter.MileageFrom && x.Mileage <= filter.MileageTo &&
            ((!x.CurrentHighBid.HasValue && !filter.CurrentHighBidFrom.HasValue) || 
                (x.CurrentHighBid >= filter.CurrentHighBidFrom && x.CurrentHighBid <= filter.CurrentHighBidTo)) &&
            x.ReservePrice >= filter.ReservePriceFrom && x.ReservePrice <= filter.ReservePriceTo && 
            (filter.FilterByStatus == null || x.Status == filter.FilterByStatus) &&
            (filter.FilterByMake == null || x.Make.ToLower().Contains(filter.FilterByMake.ToLower())) &&
            (filter.FilterByColor == null || x.Color.ToLower().Contains(filter.FilterByColor.ToLower()))
        );

        var executionResult = await query.ExecuteAsync(cancellationToken);
        var resultDtos = _mapper.Map<IEnumerable<AuctionItem>, IEnumerable<AuctionItemDto>>(executionResult.Results);

        return Ok(new FilteredResultsResponse<AuctionItemDto>(resultDtos, executionResult.TotalCount, executionResult.PageCount));
    }
}