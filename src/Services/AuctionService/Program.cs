using AuctionService;
using AuctionService.Data;
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMassTransit(x => 
{
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

        cfg.ConfigureEndpoints(context);
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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

class EnvironmentNameFormatter : IEntityNameFormatter
    {
        public string FormatEntityName<T>()
        {
            return typeof(T).Name;
        }
    }
