using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// In-memory storage to simulate localStorage
var storage = new Dictionary<string, string>();
builder.Services.AddSingleton(storage);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// GET /storage/{key} - Retrieves a value by key (localStorage.getItem)
// Returns 200 OK with the value, or 404 Not Found if key doesn't exist
app.MapGet("/storage/{key}", (string key, Dictionary<string, string> storage) =>
{
    if (storage.TryGetValue(key, out var value))
    {
        return Results.Ok(new { key, value });
    }
    return Results.NotFound(new { message = $"Key '{key}' not found" });
});

// PUT /storage/{key} - Stores or updates a value by key (localStorage.setItem)
// Returns 200 OK if updated, or 201 Created if new key was created
app.MapPut("/storage/{key}", (string key, [FromBody] StorageValue storageValue, Dictionary<string, string> storage) =>
{
    bool isNewKey = !storage.ContainsKey(key);
    storage[key] = storageValue.Value;

    if (isNewKey)
    {
        return Results.Created($"/storage/{key}", new { key, value = storageValue.Value });
    }
    return Results.Ok(new { key, value = storageValue.Value });
});

app.Run();

// Model for the storage value payload
record StorageValue(string Value);