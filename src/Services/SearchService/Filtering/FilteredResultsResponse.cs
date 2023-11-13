namespace SearchService.Filtering;

public record FilteredResultsResponse<T>(IEnumerable<T> Results, long TotalCount, int PageCount) where T : class;