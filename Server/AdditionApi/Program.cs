using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");
var storage = new ConcurrentDictionary<string, string>();
app.MapGet("/storage/{key}", (string key) =>
{
    if (storage.TryGetValue(key, out var value))
    {
        return Results.Ok(value);
    }
    return Results.NotFound();
});
app.MapPost("/storage", (StorageItem item) =>
{
    if (string.IsNullOrWhiteSpace(item.Key) || item.Value == null)
    {
        return Results.BadRequest(new { message = "Key value not matched" });
    }
    storage[item.Key] = item.Value;
    return Results.Created($"/storage/{item.Key}", item.Value);
});

app.Run();




