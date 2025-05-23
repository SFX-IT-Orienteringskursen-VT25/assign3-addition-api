
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

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

// In-memory storage
var localStore = new ConcurrentDictionary<string, string>();

app.MapGet("/orders/{key}", (string key) =>
{
    if (localStore.TryGetValue(key, out var value))
    {
        return Results.Ok(value);
    }
    return Results.NotFound();
});


app.MapPost("/orders", ([FromBody] KeyValuePair<string, string> item) =>
{
    localStore[item.Key] = item.Value;
    return Results.Created($"/orders/{item.Key}", item);
});

app.Run();