using AdditionApi;
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



// app.MapGet("/", () =>
// {
//     return "Hello World!";
// });

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//             new WeatherForecast
//             (
//                 DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                 Random.Shared.Next(-20, 55),
//                 WeatherForecastStatus.Summaries[Random.Shared.Next(WeatherForecastStatus.Summaries.Length)]
//             ))
//         .ToArray();
//     return forecast;
// });



// create two endpoints that are suitable for replacing localStorage. one for localStorage.setItem and another for localStorage.getItem
// app.MapPost("/localStorage/setItem", ([FromBody] LocalStorageItem item) =>
// {
//     return Results.Ok("Item stored");
// });
// app.MapGet("/localStorage/getItem", ([FromQuery] LocalStorageItem item) =>
// {
//     return Results.Ok("Item retrieved");
// });
var store = new ConcurrentDictionary<string, string?>();

app.MapGet("/localStorage/getItem", ([FromRoute] string key) =>
{
    return store.TryGetValue(key, out var value)
        ? Results.Ok(new { value })
        : Results.Ok(new { value = (string?)null });
});


app.MapPut("/localStorage/setItem", async ([FromRoute] string key, HttpRequest request) =>
{
    var body = await request.ReadFromJsonAsync<SetItemRequest>();

    store[key] = body.Value;
    return Results.NoContent();
});
app.Run();
public record SetItemRequest(string? Value);