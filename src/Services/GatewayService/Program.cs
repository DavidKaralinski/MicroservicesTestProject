using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = configuration["IdentityServiceUrl"];
        opt.TokenValidationParameters.ValidateAudience = false;
        opt.TokenValidationParameters.NameClaimType = "user_name";
        opt.RequireHttpsMetadata = false;
    });

builder.Services.AddCors(opt => {
    opt.AddPolicy("corsPolicy", b => {
        b.AllowAnyHeader().AllowAnyMethod().WithOrigins(configuration["ClientAppUrl"] ?? "");
    });
});

var app = builder.Build();

app.MapReverseProxy();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
