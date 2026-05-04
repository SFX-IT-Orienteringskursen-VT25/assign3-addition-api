using AdditionApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var storage = new Dictionary<string, string>();

app.MapPut("/storage/{key}", (string key, StorageItem item) =>
{
    storage[key] = item.Value;

    return Results.NoContent();
});

app.MapGet("/storage/{key}", (string key) =>
{
    if (storage.TryGetValue(key, out var value))
    {
        return Results.Ok(new StorageItem(value));
    }

    return Results.NotFound();
});

app.Run();

public record StorageItem(string Value);