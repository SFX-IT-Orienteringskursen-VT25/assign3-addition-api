using AdditionApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Create a dictionary to store our data (replaces localStorage)
var storage = new Dictionary<string, string>();

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

// POST endpoint to save data (insted of localStorage.setItem)
app.MapPost("/api/storage", ([FromBody] StorageItem item) =>
{
    if (string.IsNullOrEmpty(item.Key))
    {
        return Results.BadRequest("Key cannot be empty");
    }

    storage[item.Key] = item.Value;
    return Results.Ok(new { key = item.Key, value = item.Value });
});

// GET endpoint to retrieve data (insted of localStorage.getItem)
app.MapGet("/api/storage/{key}", (string key) =>
{
    if (storage.ContainsKey(key))
    {
        return Results.Ok(new { key = key, value = storage[key] });
    }

    return Results.NotFound(new { message = $"Key '{key}' not found" });
});

app.Run();

// Data model for storage requests
record StorageItem(string Key, string Value);