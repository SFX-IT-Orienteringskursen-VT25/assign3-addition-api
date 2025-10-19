using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

//Add Cors Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy
            .AllowAnyOrigin() 
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//Apply the Cors policy
app.UseCors("AllowLocalhost");

// --- In-memory list to store numbers ---
List<int> numbers = [];

app.MapGet("/", () =>
{
    return "Hello World!";
});

app.MapGet("/numbers", () =>
{
    
        return Results.Json(new { savedNumbers = numbers }, statusCode:200);

});

app.MapPost("/numbers", (NumberInput req) =>
{
    numbers.Add(req.number);

    return Results.Json(new
    {
        savedNumbers = numbers
    }, statusCode :200);
});


app.Run();

public record NumberInput(int number);
