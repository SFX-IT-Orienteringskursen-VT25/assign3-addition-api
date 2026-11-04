using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var storage = new ConcurrentDictionary<string, string>();

app.MapPut("/storage/{key}", (string key, [FromBody] string value) =>
{
    storage[key] = value;
    return Results.NoContent();
});

app.MapGet("/storage/{key}", (string key) =>
{
    return storage.TryGetValue(key, out var value)
        ? Results.Ok(value)
        : Results.NotFound();
});

app.Run();