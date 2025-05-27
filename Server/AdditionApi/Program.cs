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



var storedNumbers = new List<int>();

app.MapGet("/api/numbers", () =>
{
    var result = new
    {
        numbers = storedNumbers,
        sum = storedNumbers.Sum()
    };
    return Results.Ok(result); // 200 OK with data
});

app.MapPost("/api/numbers", ([FromBody] int number) =>
{
    storedNumbers.Add(number);
    var result = new
    {
        numbers = storedNumbers,
        sum = storedNumbers.Sum()
    };
    return Results.Created("/api/numbers", result); // 201 Created
});


app.Run();