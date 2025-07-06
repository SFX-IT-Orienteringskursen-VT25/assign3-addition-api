using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

// File path for persistent storage
var storageFile = "storage.json";

// Load storage from file if it exists
ConcurrentDictionary<string, string> storage;
if (File.Exists(storageFile))
{
    var json = File.ReadAllText(storageFile);
    var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
    storage = new ConcurrentDictionary<string, string>(dict);
}
else
{
    storage = new ConcurrentDictionary<string, string>();
}

void SaveStorage()
{
    var json = JsonSerializer.Serialize(storage);
    File.WriteAllText(storageFile, json);
}

// POST /storage/{key} - setItem
app.MapPost("/storage/{key}", ([FromRoute] string key, [FromBody] ValueDto dto) =>
{
    if (dto == null || dto.Value == null)
    {
        return Results.BadRequest(new { error = "Value is required" });
    }
    storage[key] = dto.Value;
    SaveStorage();
    return Results.StatusCode(201);
});

// GET /storage/{key} - getItem
app.MapGet("/storage/{key}", ([FromRoute] string key) =>
{
    if (storage.TryGetValue(key, out var value))
    {
        return Results.Ok(new { value });
    }
    return Results.NotFound(new { error = "Key not found" });
});

app.Run();

public class ValueDto
{
    public string? Value { get; set; }
}