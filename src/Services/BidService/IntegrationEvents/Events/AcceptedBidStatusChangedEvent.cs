namespace IntegrationEvents.Events;

public record AcceptedBidStatusChangedEvent(string AuctionId, string BidderName, DateTime BidTime, int BidAmount, string NewBidStatus);