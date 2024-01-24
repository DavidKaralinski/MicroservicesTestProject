namespace IntegrationEvents.Events;

public record AuctionUpdatedEvent()
{
    public Guid Id { get; init; }
    public string Make { get; init; }
    public string Model { get; init; }
    public int Year { get; init; }   
    public string Color { get; init; }
    public int Mileage { get; init; }
    public string? Winner { get; set; }
    public int? SoldAmount { get; set; }
    public int? CurrentHighBid { get; set; }
    public string Status { get; set; }
}