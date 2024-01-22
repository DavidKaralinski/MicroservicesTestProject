namespace IntegrationEvents.Events;

public record AuctionCreatedEvent()
{
    public Guid Id { get; init; }
    public int ReservePrice { get; init; }
    public string SellerName { get; init; }  
    public string? Winner { get; init; }
    public int? SoldAmount { get; init; }
    public DateTime? AuctionEnd { get; init; }
}