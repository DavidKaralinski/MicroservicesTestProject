export type PaginatedResponse<T> = {
    results?: T[];
    totalCount: number;
    pageCount: number;
}