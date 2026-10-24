using AdditionApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var storedNumbers = new List<int>();

app.MapGet("/numbers", () => Results.Ok(storedNumbers))
   .WithName("GetNumbers");

app.MapPost("/numbers", (List<int> numbers) =>
{
    if (numbers == null)
    {
        return Results.BadRequest("Geçersiz veri formatı.");
    }
    
    storedNumbers = numbers;
    return Results.Ok(storedNumbers);
})
.WithName("SaveNumbers");

app.Run();