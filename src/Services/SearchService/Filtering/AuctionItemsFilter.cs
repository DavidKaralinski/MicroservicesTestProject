using SearchService.Constants;

namespace SearchService.Filtering;

public record AuctionItemsFilter(
    string? SearchTerm, string? OrderBy,
    string? FilterBy, string? Seller,
    string? Winner, 
    int PageNumber = 1, int PageSize = 10);
