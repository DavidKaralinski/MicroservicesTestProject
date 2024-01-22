using Microsoft.EntityFrameworkCore;

namespace BidService.Data;

public static class DbInitializer
{
    public static void InitializeDb(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BidDbContext>();
        context.Database.Migrate();
    }
}