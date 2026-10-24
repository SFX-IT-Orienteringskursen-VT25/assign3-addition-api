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

// in memory replacement for the browsers localStorage (lost on restart)
var storage = new ConcurrentDictionary<string, string>();

// localStorage.setItem(key, value)
// PUT: the client chooses the key, repeating the call gives the same result
// 204 No Content: success therefornothing useful to send back
app.MapPut("/storage/{key}", (string key, [FromBody] string value) =>
{
    storage[key] = value;
    return Results.NoContent();
});

// localStorage.getItem(key)
// GET: read only and safe
// 200 OK + the value if it exists or 404 Not Found if not (getItem returns null).
app.MapGet("/storage/{key}", (string key) =>
    storage.TryGetValue(key, out var value)
        ? Results.Ok(value)
        : Results.NotFound());

app.Run();
