using IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.IntegrationEvents.Consumers;

public class AuctionCreatedEventConsumer : IConsumer<AuctionCreatedEvent>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public AuctionCreatedEventConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<AuctionCreatedEvent> context)
    {
        await _hubContext.Clients.All.SendAsync("AuctionCreated", context.Message);
    }
}