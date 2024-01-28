using BidService.Data;
using BidService.IntegrationEvents.Consumers;
using BidService.Services;
using IntegrationEvents.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddDbContext<BidDbContext>(opt =>
{
    opt.UseNpgsql(configuration.GetConnectionString("BidsDb"));
}
);

builder.Services.AddMassTransit(x => 
{
    x.AddConsumer<AuctionCreatedEventConsumer>();
    x.AddConsumer<AuctionDeletedEventConsumer>();

    x.UsingRabbitMq((context, cfg) => 
    {
        cfg.Host(configuration.GetValue("RabbitMq:Host", "localhost"), "/", x => 
        {
            x.Username(configuration.GetValue("Rabbitmq:User", "guest"));
            x.Password(configuration.GetValue("Rabbitmq:Password", "guest"));
        });

        cfg.Message<AuctionCreatedEvent>(m => m.SetEntityName("auction-created"));
        cfg.Message<AuctionDeletedEvent>(m => m.SetEntityName("auction-deleted"));
        cfg.Message<AuctionFinishedEvent>(m => m.SetEntityName("auction-finished"));
        cfg.Message<BidPlacedEvent>(m => m.SetEntityName("bid-placed"));
        cfg.Message<AcceptedBidStatusChangedEvent>(m => m.SetEntityName("accepted-bid-status-changed"));

        cfg.ReceiveEndpoint("bids-auction-created", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionCreatedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("bid-auction-deleted", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionDeletedEventConsumer>(context);
        });
    });
});

builder.Services.AddAuthentication().AddJwtBearer(opt => 
{
    opt.Authority = configuration["IdentityServiceUrl"];
    opt.RequireHttpsMetadata = false;
    opt.TokenValidationParameters.ValidateAudience = false;
    opt.TokenValidationParameters.NameClaimType = "user_name";
});

builder.Services.AddControllers();
builder.Services.AddScoped<IAuctionsGrpcService, AuctionsGrpcService>();
builder.Services.AddHostedService<FinishedAuctionsCheckerBackgroundService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.InitializeDb();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
