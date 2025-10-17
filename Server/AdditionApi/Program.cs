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



app.MapGet("/", () =>
{
    return "Hello World!";
});

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                WeatherForecastStatus.Summaries[Random.Shared.Next(WeatherForecastStatus.Summaries.Length)]
            ))
        .ToArray();
    return forecast;
});



// create two endpoints that are suitable for replacing localStorage. one for localStorage.setItem and another for localStorage.getItem
app.MapPost("/localStorage/setItem", ([FromBody] LocalStorageItem item) =>
{
    return Results.Ok("Item stored");
});
app.MapGet("/localStorage/getItem", ([FromQuery] LocalStorageItem item) =>
{
    return Results.Ok("Item retrieved");
});
