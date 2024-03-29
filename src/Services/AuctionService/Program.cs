using AuctionService;
using AuctionService.Data;
using AuctionService.Grpc;
using AuctionService.IntegrationEvents.Consumers;
using IntegrationEvents.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(configuration.GetConnectionString("AuctionsDb"));
}
);

builder.Services.AddControllers();
builder.Services.AddGrpc();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMassTransit(x => 
{
    x.AddConsumer<BidPlacedEventConsumer>();
    x.AddConsumer<AuctionFinishedEventConsumer>();

    x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(10);
        o.UsePostgres();
        o.UseBusOutbox();
    });

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

        cfg.ReceiveEndpoint("auctions-auction-finished", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionFinishedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("auctions-bid-placed", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<BidPlacedEventConsumer>(context);
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

var app = builder.Build();

app.InitializeDb();
// Configure the HTTP request pipeline.

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<AuctionsGrpcService>();
app.MapControllers();

app.Run();
