using SearchService.Constants;

namespace SearchService.Filtering;

public record AuctionItemsFilter(string? FilterByMake, string? FilterByColor, string? FilterByStatus,
    string? OrderableFieldName, OrderingDirection? OrderingDirection,
    int? MileageFrom = 0, int? MileageTo = int.MaxValue,
    int? CurrentHighBidFrom = null, int? CurrentHighBidTo = int.MaxValue,
    int? ReservePriceFrom = 0, int? ReservePriceTo = int.MaxValue,
    int? YearFrom = 0, int? YearTo = int.MaxValue,
    int? EndingInLessThan = null,
    int PageNumber = 1, int PageSize = 10);
