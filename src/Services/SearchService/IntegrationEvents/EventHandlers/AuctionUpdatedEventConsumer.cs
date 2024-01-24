using IntegrationEvents.Events;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.IntegrationEvents.EventHandlers;

public class AuctionUpdatedEventConsumer : IConsumer<AuctionUpdatedEvent>
{
    public async Task Consume(ConsumeContext<AuctionUpdatedEvent> context)
    {
        await DB.Update<AuctionItem>()
            .MatchID(context.Message.Id.ToString())
            .Modify(e => e.Color, context.Message.Color)
            .Modify(e => e.Model, context.Message.Model)
            .Modify(e => e.Make, context.Message.Make)
            .Modify(e => e.Mileage, context.Message.Mileage)
            .Modify(e => e.Year, context.Message.Year)
            .Modify(e => e.Winner, context.Message.Winner)
            .Modify(e => e.SoldAmount, context.Message.SoldAmount)
            .Modify(e => e.Status, context.Message.Status)
            .ExecuteAsync();
    }
}