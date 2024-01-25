using IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.IntegrationEvents.Consumers;

public class AcceptedBidStatusChangedEventConsumer : IConsumer<AcceptedBidStatusChangedEvent>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public AcceptedBidStatusChangedEventConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<AcceptedBidStatusChangedEvent> context)
    {
        await _hubContext.Clients.All.SendAsync("AcceptedBidStatusChanged", context.Message);
    }
}