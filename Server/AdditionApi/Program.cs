
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// In-memory storage (simulates localStorage)
Dictionary<string, string> localStorage = new();

// POST /storage - simulate localStorage.setItem
app.MapPost("/storage", ([FromBody] KeyValuePair<string, string> payload) =>
{
    if (string.IsNullOrEmpty(payload.Key) || payload.Value == null)
    {
        return Results.BadRequest("Both key and value are required.");
    }

    localStorage[payload.Key] = payload.Value;
    return Results.Created($"/storage/{payload.Key}", new { message = "Stored successfully" });
});

// GET /storage/{key} - simulate localStorage.getItem
app.MapGet("/storage/{key}", ([FromRoute] string key) =>
{
    if (localStorage.TryGetValue(key, out var value))
    {
        return Results.Ok(new { value });
    }

    return Results.NotFound(new { error = "Key not found" });
});

app.Run();