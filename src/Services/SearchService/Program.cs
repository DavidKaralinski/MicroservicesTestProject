using SearchService.Data;
using MassTransit;
using SearchService.IntegrationEvents.EventHandlers;
using IntegrationEvents.Events;

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
    //x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

    x.UsingRabbitMq((context, cfg) => 
    {
        cfg.Message<AuctionCreatedEvent>(m => m.SetEntityName("auction-created"));

        cfg.ReceiveEndpoint("search-auction-created", e => 
        {
            e.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10)));
            e.ConfigureConsumer<AuctionCreatedEventConsumer>(context);
        });

        //cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
await app.InitializeDatabaseAsync();

app.Run();