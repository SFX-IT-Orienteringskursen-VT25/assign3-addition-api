using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var store = new ConcurrentDictionary<string, string>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Get / - Default route
app.MapGet("/", () =>
{
    return "assign3-addition-api";
});

// POST /api/storage - equivalent to localStorage.setItem
app.MapPost("/api/storage", (KeyValue item) =>
{
    if (string.IsNullOrWhiteSpace(item.Key))
        return Results.BadRequest("Key is required.");

    store[item.Key] = item.Value;
    return Results.StatusCode(201); // Created
});

// GET /api/storage/{key} - equivalent to localStorage.getItem
app.MapGet("/api/storage/{key}", (string key) =>
{
    return store.TryGetValue(key, out var value)
        ? Results.Ok(new { key, value })
        : Results.NotFound();
});

// run the app
app.Run();

// Record type for input model
record KeyValue(string Key, string Value);