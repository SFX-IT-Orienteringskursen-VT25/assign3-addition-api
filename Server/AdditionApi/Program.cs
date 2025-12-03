using AdditionApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// In-memory storage to replace localStorage
var storage = new Dictionary<string, string>();

// POST /data - Replace localStorage.setItem(key, value)
app.MapPost("/data", ([FromBody] StoredData data) =>
{
    if (string.IsNullOrWhiteSpace(data.Key) || data.Value == null)
    {
        return Results.BadRequest("Key and Value are required");
    }

    storage[data.Key] = data.Value;
    return Results.Created($"/data/{data.Key}", data);
});

// GET /data/{key} - Replace localStorage.getItem(key)
app.MapGet("/data/{key}", (string key) =>
{
    if (storage.TryGetValue(key, out var value))
    {
        return Results.Ok(new StoredData(key, value));
    }

    return Results.NotFound($"Key '{key}' not found");
});

app.Run();