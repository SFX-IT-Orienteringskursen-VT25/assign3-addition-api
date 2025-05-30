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

app.MapPost("/order", ([FromBody] Order order) =>
{
    if (order.Item == null)
    {
        return Results.BadRequest("Must provide an item");
    }

    return Results.Ok("Order received");
});
app.MapPut("/order", ([FromBody] Order order) =>
{
    return Results.Ok("Order has been updated");
});
app.MapDelete("/order", ([FromBody] Order order) => Results.NoContent());

app.Run();