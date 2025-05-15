using AdditionApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

// Storage endpoints
app.MapPut("/storage/{key}", ([FromRoute] string key, [FromBody] StorageValue value) =>
{
    Storage.Set(key, value.Value);
    return Results.Ok(new { success = true });
});

app.MapGet("/storage/{key}", ([FromRoute] string key) =>
{
    var value = Storage.Get(key);
    if (value is null)
        return Results.NotFound();
    return Results.Ok(new StorageValue { Value = value });
});

// WeatherForecast endpoint
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            WeatherForecastStatus.Summaries[Random.Shared.Next(WeatherForecastStatus.Summaries.Length)]
        ))
    .ToArray();
    return forecast;
});

// Order endpoints
app.MapPost("/order", ([FromBody] Order order) =>
{
    if (string.IsNullOrEmpty(order.Item))
        return Results.BadRequest("Must provide an item");
    return Results.Ok("Order received");
});

app.MapPut("/order", ([FromBody] Order order) =>
{
    return Results.Ok("Order has been updated");
});

app.MapDelete("/order", ([FromBody] Order order) =>
{
    return Results.NoContent();
});

app.Run();
