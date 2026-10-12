using System.Collections.Concurrent;
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

// Works like the browser's localStorage: key -> string value
var storage = new ConcurrentDictionary<string, string>();

// Replaces localStorage.setItem(key, value)
// PUT because it creates or overwrites the value for the key (idempotent).
// 204 No Content because setItem returns nothing.
app.MapPut("/storage/{key}", (string key, [FromBody] string value) =>
{
    storage[key] = value;
    return Results.NoContent();
});

// Replaces localStorage.getItem(key)
// GET because it only reads.
// 200 OK with the value, or 404 Not Found when the key does not exist
// (localStorage.getItem returns null in that case).
app.MapGet("/storage/{key}", (string key) =>
{
    return storage.TryGetValue(key, out var value)
        ? Results.Ok(value)
        : Results.NotFound();
});

app.Run();
