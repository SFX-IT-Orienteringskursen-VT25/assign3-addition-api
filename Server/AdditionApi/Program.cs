using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var storage = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);

app.MapPut("/api/storage/{key}", (string key, StorageValue request) =>
{
    if (request.Value is null)
    {
        return Results.BadRequest("Value must not be null.");
    }

    storage[key] = request.Value;

    return Results.NoContent();
});

app.MapGet("/api/storage/{key}", (string key) =>
    storage.TryGetValue(key, out var value)
        ? Results.Ok(new StorageValue(value))
        : Results.NotFound());

app.Run();

record StorageValue(string? Value);