namespace IntegrationEvents.Events;

public record AuctionUpdatedEvent(Guid Id, string Male, string Make, string Model, int Year, string Color, int Mileage);