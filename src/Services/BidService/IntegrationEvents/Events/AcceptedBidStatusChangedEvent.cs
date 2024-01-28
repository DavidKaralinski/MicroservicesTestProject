namespace IntegrationEvents.Events;

public record AcceptedBidStatusChangedEvent(long BidId, string AuctionId, string BidderName, DateTime BidTime, int BidAmount, string NewBidStatus);