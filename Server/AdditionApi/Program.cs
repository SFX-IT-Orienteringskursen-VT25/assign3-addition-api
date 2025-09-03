using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container - simplified for .NET 7.0
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseHttpsRedirection();

// Simple in-memory storage for the numbers array
var numbersStorage = new List<int>();

app.MapGet("/", () =>
{
    return "Hello World!";
});

// GET endpoint - replaces localStorage.getItem('enteredNumbers')
app.MapGet("/storage/enteredNumbers", () =>
{
    return Results.Ok(new { numbers = numbersStorage });
});

// PUT endpoint - replaces localStorage.setItem('enteredNumbers', JSON.stringify(numbers))
app.MapPut("/storage/enteredNumbers", ([FromBody] NumbersRequest request) =>
{
    numbersStorage.Clear();
    numbersStorage.AddRange(request.Numbers);
    return Results.Ok(new { numbers = numbersStorage });
});

app.Run();

// Record for the numbers request
public record NumbersRequest(int[] Numbers);