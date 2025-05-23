using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// 用于存储 key-value 对象，线程安全
var storage = new ConcurrentDictionary<string, string>();

// POST /storage - 模拟 localStorage.setItem(key, value)
app.MapPost("/storage", ([FromBody] KeyValuePair<string, string> data) =>
{
    if (string.IsNullOrWhiteSpace(data.Key))
    {
        return Results.BadRequest("Key is required.");
    }

    storage[data.Key] = data.Value ?? "";
    return Results.Created($"/storage/{data.Key}", new { message = "Item stored" });
});

// GET /storage/{key} - 模拟 localStorage.getItem(key)
app.MapGet("/storage/{key}", ([FromRoute] string key) =>
{
    if (storage.TryGetValue(key, out var value))
    {
        return Results.Ok(new { key, value });
    }

    return Results.NotFound(new { message = "Key not found" });
});

app.Run();
