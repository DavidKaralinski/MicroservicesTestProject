using BidService.Entities;

namespace BidService.Services;

public interface IAuctionsGrpcService
{
    Task<Auction> GetAuctionByIdAsync(string auctionId, CancellationToken cancellationToken = default);
}