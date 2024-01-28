using IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.IntegrationEvents.Consumers;

public class AuctionDeletedEventConsumer : IConsumer<AuctionDeletedEvent>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public AuctionDeletedEventConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<AuctionDeletedEvent> context)
    {
        await _hubContext.Clients.All.SendAsync("AuctionDeleted", context.Message);
    }
}