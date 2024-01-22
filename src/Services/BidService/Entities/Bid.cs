using BidService.Enums;

namespace BidService.Entities;

public class Bid
{
    public long Id { get; set; }
    public string AuctionId { get; set; }
    public int Amount { get; set; }
    public DateTime BidTime { get; set; } = DateTime.UtcNow;
    public BidStatus Status { get; set; }
    public string BidderName { get; set; }
}