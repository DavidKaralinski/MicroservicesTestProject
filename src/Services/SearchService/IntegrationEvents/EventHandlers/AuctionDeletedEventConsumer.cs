using IntegrationEvents.Events;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.IntegrationEvents.EventHandlers;

public class AuctionDeletedEventConsumer : IConsumer<AuctionDeletedEvent>
{
    public AuctionDeletedEventConsumer()
    {
    }

    public async Task Consume(ConsumeContext<AuctionDeletedEvent> context)
    {
        var entity = await DB.Find<AuctionItem>().OneAsync(context.Message.Id.ToString());

        if(entity is null)
        {
            return;
        }

        await entity.DeleteAsync();
    }
}