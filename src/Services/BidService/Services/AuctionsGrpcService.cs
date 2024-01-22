using BidService.Entities;
using Grpc.Net.Client;
using static BidService.GrpcAuctions;

namespace BidService.Services;

public class AuctionsGrpcService : IAuctionsGrpcService
{
    private readonly ILogger<AuctionsGrpcService> _logger;
    private readonly IConfiguration _configuration;

    public AuctionsGrpcService(ILogger<AuctionsGrpcService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<Auction> GetAuctionByIdAsync(string auctionId, CancellationToken cancellationToken = default)
    {
        var auctionsGrpcUrl = _configuration["Grpc:AuctionsGrpcUrl"];

        if(String.IsNullOrEmpty(auctionsGrpcUrl))
        {
            _logger.LogError("------ Cannot find auctions grpc service url in the configuration");
            throw new ArgumentException();
        }

        var grpcChannel = GrpcChannel.ForAddress(auctionsGrpcUrl);
        var client = new GrpcAuctionsClient(grpcChannel);

        try
        {
            var auctionResponse = await client.GetAuctionByIdAsync(new GetAuctionByIdRequest{ Id = auctionId }, cancellationToken: cancellationToken);
            return new Auction() 
            {
                Id = auctionResponse.Auction.Id,
                ReservePrice = auctionResponse.Auction.ReservePrice,
                AuctionEnd = DateTime.Parse(auctionResponse.Auction.AuctionEnd),
                SellerName = auctionResponse.Auction.SellerName
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("------ Error occured while getting auction by its id. Error message = {message}", ex.Message);
            throw;
        }
    }
}