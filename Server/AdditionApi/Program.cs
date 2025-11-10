using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Optional: Swagger/OpenAPI (remove if teacher doesn't want)
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// In-memory key-value store (simulates localStorage)
Dictionary<string, string> localStorage = new();

// POST /storage -> simulate localStorage.setItem(key, value)
app.MapPost("/storage", ([FromBody] StorageItem payload) =>
{
    if (payload is null || string.IsNullOrEmpty(payload.Key) || payload.Value is null)
    {
        return Results.BadRequest(new { error = "Both 'key' and 'value' are required." });
    }

    localStorage[payload.Key] = payload.Value;
    return Results.Created($"/storage/{payload.Key}", new { message = "Stored successfully" });
});

// GET /storage/{key} -> simulate localStorage.getItem(key)
app.MapGet("/storage/{key}", (string key) =>
{
    if (localStorage.TryGetValue(key, out var value))
    {
        return Results.Ok(new { value });
    }

    return Results.NotFound(new { error = "Key not found" });
});

app.Run();

// Request/response model
public record StorageItem(string Key, string Value);
