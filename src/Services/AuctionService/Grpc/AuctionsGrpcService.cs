using AuctionService.Data;
using AutoMapper;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Grpc;

public class AuctionsGrpcService : GrpcAuctions.GrpcAuctionsBase
{
    private readonly ILogger<AuctionsGrpcService> _logger;
    private readonly IMapper _mapper;
    private readonly AuctionDbContext _dbContext;


    public AuctionsGrpcService(ILogger<AuctionsGrpcService> logger,
        IMapper mapper,
        AuctionDbContext dbContext)
    {
        _logger = logger;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public override async Task<GrpcAuctionResponse> GetAuctionById(GetAuctionByIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation("----- Received grpc request of type {requestType}", typeof(GetAuctionByIdRequest).Name);

        try
        {
            var auction = await _dbContext.Auctions.FirstOrDefaultAsync(x => x.Id.ToString() == request.Id);
            var auctionResponseModel = auction is null ? null : new GrpcAuctionModel()
            {
                AuctionEnd = auction.AuctionEnd.ToString(),
                ReservePrice = auction.ReservePrice,
                SellerName = auction.SellerName,
                Id = auction.Id.ToString()
            };

            return new GrpcAuctionResponse() { Auction = auctionResponseModel };
        }
        catch (Exception ex)
        {
            _logger.LogError("----- Error occured while handling the {requestType}. Error message = {errorMessage}", 
                typeof(GetAuctionByIdRequest).Name, ex.Message);

            throw;
        }
    }
}