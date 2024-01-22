using BidService.Data;
using IntegrationEvents.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace BidService.IntegrationEvents.Consumers;

public class AuctionCreatedEventConsumer : IConsumer<AuctionCreatedEvent>
{
    private readonly BidDbContext _dbContext;
    private readonly ILogger<AuctionCreatedEventConsumer> _logger;

    public AuctionCreatedEventConsumer(BidDbContext dbContext, ILogger<AuctionCreatedEventConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuctionCreatedEvent> context)
    {
        _logger.LogInformation("----- Consuming {eventType}", typeof(AuctionCreatedEvent).Name);

        try
        {
            if (!await _dbContext.Auctions.AnyAsync(x => x.Id == context.Message.Id.ToString()))
            {
                _dbContext.Auctions.Add(new()
                {
                    AuctionEnd = context.Message.AuctionEnd.GetValueOrDefault(),
                    ReservePrice = context.Message.ReservePrice,
                    Id = context.Message.Id.ToString(),
                    SellerName = context.Message.SellerName
                });

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("----- Successfully handled {eventType}", typeof(AuctionCreatedEvent).Name);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("----- Error handling {eventType}. Error message: {errorMessage}",
                 typeof(AuctionCreatedEvent).Name,
                 e.Message);

            throw;
        }
    }
}