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

app.MapGet("/api/numbers", () =>
{
    int[] numbers = { 5, 10, 3 };

    return Results.Ok(numbers);
});

app.MapPut("/api/numbers", (int[] numbers) =>
{
    Console.WriteLine($"Received numbers: {string.Join(", ", numbers)}");

    return Results.NoContent();
});

app.Run();