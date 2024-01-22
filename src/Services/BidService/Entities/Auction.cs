namespace BidService.Entities;

public class Auction
{
    public string Id { get; set; }
    public DateTime AuctionEnd { get; set; }
    public string SellerName { get; set; }
    public int ReservePrice { get; set; }
    public bool IsFinished { get; set; }
}