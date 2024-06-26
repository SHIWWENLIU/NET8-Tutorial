﻿using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Driver;
using MongoDB.Entities;

namespace SearchService;

public class DbInitializer
{
    public static async Task InitDb (WebApplication app)
    {
        //call static function of DB
        await DB.InitAsync("SearchDb", MongoClientSettings.
            FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();

        if(count == 0)
        {
            var  itemData = await File.ReadAllTextAsync("Data/auctions.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var  item = JsonSerializer.Deserialize<List<Item>> (itemData, options);
            await DB.SaveAsync(item);
        }
    }
}
