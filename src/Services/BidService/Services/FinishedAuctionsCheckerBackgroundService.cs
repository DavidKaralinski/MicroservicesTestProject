
using BidService.Data;
using BidService.Enums;
using IntegrationEvents.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace BidService.Services;

public class FinishedAuctionsCheckerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FinishedAuctionsCheckerBackgroundService> _logger;

    public FinishedAuctionsCheckerBackgroundService(IServiceProvider serviceProvider,
        ILogger<FinishedAuctionsCheckerBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("----- Finished auctions checker process is starting");
        stoppingToken.Register(() => _logger.LogInformation("----- Auctions checker is stopping"));
        
        while(!stoppingToken.IsCancellationRequested)
        {
            await CheckAuctions(stoppingToken);
            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task CheckAuctions(CancellationToken stoppingToken)
    {
        var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BidDbContext>();
        var finishedAuctions = await dbContext.Auctions.Where(x => !x.IsFinished && x.AuctionEnd < DateTime.UtcNow).ToListAsync(stoppingToken);

        if(!finishedAuctions.Any())
        {
            _logger.LogInformation("----- No finished auctions have been found.");
            return;
        }

        _logger.LogInformation("----- {count} finished auctions have been found", finishedAuctions.Count());
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        foreach(var finishedAuction in finishedAuctions)
        {
            var highestBid = await dbContext.Bids
                .Where(x => x.AuctionId == finishedAuction.Id)
                .OrderByDescending(x => x.Amount)
                .FirstOrDefaultAsync(stoppingToken);

            var auctionFinishedEvent = new AuctionFinishedEvent(finishedAuction.Id,
                 highestBid is not null && highestBid!.Status == BidStatus.Accepted,
                 highestBid?.BidderName ?? "",
                 finishedAuction.SellerName,
                 highestBid?.Amount);
            
            finishedAuction.IsFinished = true;
            await publishEndpoint.Publish(auctionFinishedEvent, stoppingToken);
        }

        await dbContext.SaveChangesAsync(stoppingToken);
    }
}