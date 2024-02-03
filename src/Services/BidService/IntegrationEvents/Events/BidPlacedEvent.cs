using BidService.Enums;

namespace IntegrationEvents.Events;

public record BidPlacedEvent(string AuctionId, string BidderName, DateTime BidTime, int BidAmount, string BidStatus, long BidId);