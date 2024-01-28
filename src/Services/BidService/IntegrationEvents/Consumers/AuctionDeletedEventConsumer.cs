using BidService.Data;
using IntegrationEvents.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace BidService.IntegrationEvents.Consumers;

public class AuctionDeletedEventConsumer : IConsumer<AuctionDeletedEvent>
{
    private readonly BidDbContext _dbContext;
    private readonly ILogger<AuctionDeletedEventConsumer> _logger;

    public AuctionDeletedEventConsumer(BidDbContext dbContext, ILogger<AuctionDeletedEventConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuctionDeletedEvent> context)
    {
        _logger.LogInformation("----- Consuming {eventType}", typeof(AuctionDeletedEvent).Name);

        try
        {
            var auction = await _dbContext.Auctions.FirstOrDefaultAsync(x => x.Id == context.Message.Id.ToString());
            if (auction is not null)
            {
                _dbContext.Remove(auction);

                await _dbContext.SaveChangesAsync();
            }

            _logger.LogInformation("----- Successfully handled {eventType}", typeof(AuctionDeletedEvent).Name);
        }
        catch (Exception e)
        {
            _logger.LogError("----- Error handling {eventType}. Error message: {errorMessage}",
                 typeof(AuctionDeletedEvent).Name,
                 e.Message);

            throw;
        }
    }
}