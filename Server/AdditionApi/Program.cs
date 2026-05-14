using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// In-memory data store (simulates localStorage)
var dataStore = new Dictionary<string, string>();

// POST /data - Equivalent to localStorage.setItem(key, value)
app.MapPost("/data", ([FromBody] Dictionary<string, string> request) =>
{
    if (request == null || !request.ContainsKey("key") || !request.ContainsKey("value"))
    {
        return Results.BadRequest(new { error = "Must provide 'key' and 'value'" });
    }

    string key = request["key"];
    string value = request["value"];

    if (string.IsNullOrWhiteSpace(key))
    {
        return Results.BadRequest(new { error = "Key cannot be empty" });
    }

    dataStore[key] = value;
    return Results.Ok(new { key, value });
});

// GET /data/{key} - Equivalent to localStorage.getItem(key)
app.MapGet("/data/{key}", (string key) =>
{
    if (string.IsNullOrWhiteSpace(key))
    {
        return Results.BadRequest(new { error = "Key cannot be empty" });
    }

    if (dataStore.TryGetValue(key, out var value))
    {
        return Results.Ok(new { key, value });
    }

    return Results.NotFound(new { error = $"Data for key '{key}' not found" });
});

app.Run();