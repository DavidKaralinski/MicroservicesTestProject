namespace IntegrationEvents.Events;

public record AuctionFinishedEvent(string AuctionId, bool IsSold, string WinnerName, string SellerName, int? Amount);