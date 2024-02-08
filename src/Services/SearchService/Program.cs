using SearchService.Data;
using MassTransit;
using SearchService.IntegrationEvents.EventHandlers;
using IntegrationEvents.Events;
using Polly;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMassTransit(x => 
{
    x.AddConsumer<AuctionCreatedEventConsumer>();
    x.AddConsumer<AuctionUpdatedEventConsumer>();
    x.AddConsumer<AuctionDeletedEventConsumer>();
    //x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

    x.UsingRabbitMq((context, cfg) => 
    {
        cfg.UseMessageRetry(r => 
        {
            r.Handle<RabbitMqConnectionException>();
            r.Interval(5, TimeSpan.FromSeconds(10));
        });

        cfg.Host(configuration.GetValue("RabbitMq:Host", "localhost"), "/", x => 
        {
            x.Username(configuration.GetValue("Rabbitmq:User", "guest"));
            x.Password(configuration.GetValue("Rabbitmq:Password", "guest"));
        });

        cfg.Message<AuctionCreatedEvent>(m => m.SetEntityName("auction-created"));
        cfg.Message<AuctionUpdatedEvent>(m => m.SetEntityName("auction-updated"));
        cfg.Message<AuctionDeletedEvent>(m => m.SetEntityName("auction-deleted"));

        cfg.ReceiveEndpoint("search-auction-created", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionCreatedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("search-auction-updated", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionUpdatedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("search-auction-deleted", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionDeletedEventConsumer>(context);
        });

        //cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

var retryPolicy = Policy
    .Handle<TimeoutException>()
    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(10));

await retryPolicy.ExecuteAndCaptureAsync(() => app.InitializeDatabaseAsync());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();