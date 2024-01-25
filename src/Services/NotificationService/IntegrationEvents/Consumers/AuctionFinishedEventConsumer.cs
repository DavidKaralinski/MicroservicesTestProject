using IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.IntegrationEvents.Consumers;

public class AuctionFinishedEventConsumer : IConsumer<AuctionFinishedEvent>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public AuctionFinishedEventConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<AuctionFinishedEvent> context)
    {
        await _hubContext.Clients.All.SendAsync("AuctionFinished", context.Message);
    }
}