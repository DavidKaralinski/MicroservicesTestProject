using IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.IntegrationEvents.Consumers;

public class BidPlacedEventConsumer : IConsumer<BidPlacedEvent>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public BidPlacedEventConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<BidPlacedEvent> context)
    {
        await _hubContext.Clients.All.SendAsync("BidPlaced", context.Message);
    }
}