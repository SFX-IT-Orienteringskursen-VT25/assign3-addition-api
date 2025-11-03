using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add OpenAPI/Swagger if needed
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 🧠 In-memory storage (simulates localStorage)
Dictionary<string, string> localStorage = new();

// 🟢 POST /storage → simulate localStorage.setItem(key, value)
app.MapPost("/storage", ([FromBody] StorageItem payload) =>
{
    if (string.IsNullOrEmpty(payload.Key) || payload.Value is null)
    {
        return Results.BadRequest("Both key and value are required.");
    }

    localStorage[payload.Key] = payload.Value;
    return Results.Created($"/storage/{payload.Key}", new { message = "Stored successfully" });
});

// 🟢 GET /storage/{key} → simulate localStorage.getItem(key)
app.MapGet("/storage/{key}", ([FromRoute] string key) =>
{
    if (localStorage.TryGetValue(key, out var value))
    {
        return Results.Ok(new { value });
    }

    return Results.NotFound(new { error = "Key not found" });
});

app.Run();

// 🧩 Simple record to hold key-value pairs
public record StorageItem(string Key, string Value);
