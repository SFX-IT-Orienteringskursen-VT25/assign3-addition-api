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


// In-memory storage
var storage = new Dictionary<string, string>();


// GET endpoint
// Equivalent to localStorage.getItem(key)

app.MapGet("/storage/{key}", ([FromRoute] string key) =>
{
    if (!storage.ContainsKey(key))
    {
        return Results.NotFound(new
        {
            error = "Key not found"
        });
    }

    return Results.Ok(new
    {
        key = key,
        value = storage[key]
    });
});


// POST endpoint
// Equivalent to localStorage.setItem(key, value)

app.MapPost("/storage/{key}", (
    [FromRoute] string key,
    [FromBody] StorageRecord record) =>
{
    storage[key] = record.Value;

    return Results.Ok(new
    {
        message = "Data stored successfully",
        key = key,
        value = record.Value
    });
});

app.Run();


// Request body model;