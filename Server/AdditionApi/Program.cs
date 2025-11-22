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
    return "Addition API";
});

var data = new Dictionary<string, string>();

app.MapGet("/api/addition/{key}",([FromRoute] string key) =>
{
    if (data.ContainsKey(key))
    {
        return Results.Ok(key);
    }

    return Results.NotFound(StatusCodes.Status404NotFound);
});

app.MapPost("/api/addition", ([FromBody] StorageData storageData) =>
{
    if (data.ContainsKey(storageData.Key))
    {
        return Results.BadRequest(StatusCodes.Status400BadRequest);
    }
    
    data.TryAdd(key: storageData.Key , value: storageData.Value);

    return Results.Created($"/addition/{storageData.Key}", storageData.Value);
});


app.Run();