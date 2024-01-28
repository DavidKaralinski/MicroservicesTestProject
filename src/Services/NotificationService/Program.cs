using IntegrationEvents.Events;
using MassTransit;
using NotificationService.Hubs;
using NotificationService.IntegrationEvents.Consumers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddMassTransit(x => 
{
    x.AddConsumer<BidPlacedEventConsumer>();
    x.AddConsumer<AuctionFinishedEventConsumer>();
    x.AddConsumer<AuctionCreatedEventConsumer>();
    x.AddConsumer<AuctionDeletedEventConsumer>();
    x.AddConsumer<AcceptedBidStatusChangedEventConsumer>();

    x.UsingRabbitMq((context, cfg) => 
    {
        cfg.Host(configuration.GetValue("RabbitMq:Host", "localhost"), "/", host => 
        {
            host.Username(configuration.GetValue("RabbitMq:User", "guest"));
            host.Password(configuration.GetValue("RabbitMq:Pasword", "guest"));
        });

        cfg.Message<AuctionCreatedEvent>(m => m.SetEntityName("auction-created"));
        cfg.Message<AuctionUpdatedEvent>(m => m.SetEntityName("auction-updated"));
        cfg.Message<AuctionDeletedEvent>(m => m.SetEntityName("auction-deleted"));
        cfg.Message<AuctionFinishedEvent>(m => m.SetEntityName("auction-finished"));
        cfg.Message<BidPlacedEvent>(m => m.SetEntityName("bid-placed"));
        cfg.Message<AcceptedBidStatusChangedEvent>(m => m.SetEntityName("accepted-bid-status-changed"));

        cfg.ReceiveEndpoint("notifications-auction-finished", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionFinishedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("notifications-auction-created", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionCreatedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("notifications-auction-deleted", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionDeletedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("notifications-bid-placed", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<BidPlacedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("notifications-accepted-bid-status-changed", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AcceptedBidStatusChangedEventConsumer>(context);
        });
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<NotificationHub>("/notifications");

app.Run();
