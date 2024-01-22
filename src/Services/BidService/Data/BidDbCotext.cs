using BidService.Entities;
using Microsoft.EntityFrameworkCore;

namespace BidService.Data;

public class BidDbContext : DbContext
{
    public BidDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Bid> Bids { get; set; }
    public DbSet<Auction> Auctions { get; set; }
}