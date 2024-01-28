namespace IntegrationEvents.Events;

public record AuctionDeletedEvent()
{
    public Guid Id { get; init; }
}