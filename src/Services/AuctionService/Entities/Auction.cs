namespace AuctionService.Entities;

public class Auction
{
    public Guid Id {get;set;}
    public int ReservePrice { get; set; } = 0;
    public string SellerName { get; set; }  
    public string? Winner { get; set; }
    public int? SoldAmount { get; set; }
    public int? CurrentHighBid { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? AuctionEnd { get; set; }
    public AuctionStatus Status { get; set; } = AuctionStatus.Live;

    public AuctionItem Item { get; set; }
}
