using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();



app.UseHttpsRedirection();

//store data
var dataStore = new Dictionary<string, string>();

//Equivalent to localStorage.setItem(key, value)
app.MapPost("/data", ([FromBody] Dictionary<string, string> request) =>
{
    if (request == null || !request.ContainsKey("key") || !request.ContainsKey("value"))
    {
        return Results.BadRequest(new { error = "Must provide 'key' and 'value'" });
    }

    string key = request["key"];
    string value = request["value"];

    if (string.IsNullOrWhiteSpace(key))
    {
        return Results.BadRequest(new { error = "Key cannot be empty" });
    }

    dataStore[key] = value;
    return Results.Ok(new { key, value });
});

// Equivalent to localStorage.getItem(key)
app.MapGet("/data/{key}", (string key) =>
{
    if (string.IsNullOrWhiteSpace(key))
    {
        return Results.BadRequest(new { error = "Key cannot be empty" });
    }

    if (dataStore.TryGetValue(key, out var value))
    {
        return Results.Ok(new { key, value });
    }

    return Results.NotFound(new { error = $"Data for key '{key}' not found" });
});

app.Run();