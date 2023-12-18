using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Data;

public static class DbInitializer
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        await DB.InitAsync("SearchDb",
            MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("SearchDb")));

        await DB.Index<AuctionItem>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .Key(x => x.Status, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<AuctionItem>();

        if(count > 0)
        {
            return;
        }

        string? auctionItemsJson = null;
        List<AuctionItem>? auctionItems = null;

        if (File.Exists("Data/seeddata.json") && (auctionItemsJson = await File.ReadAllTextAsync("Data/seeddata.json")) is not null
            && (auctionItems = JsonSerializer.Deserialize<List<AuctionItem>>(auctionItemsJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })) is not null)
        {
            await DB.SaveAsync(auctionItems);
        }
    }
}