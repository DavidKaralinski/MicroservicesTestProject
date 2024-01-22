namespace BidService.Models;

public class BidModel
{
    public long Id { get; set; }
    public string AuctionId { get; set; }
    public int Amount { get; set; }
    public string Status { get; set; }
    public string BidderName { get; set; }
    public DateTime BidTime { get; set; }
}