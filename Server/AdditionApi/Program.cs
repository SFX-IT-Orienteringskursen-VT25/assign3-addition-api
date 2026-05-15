using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var storage = new Dictionary<string, string>();

app.MapPut("/storage/{key}", (string key, [FromBody] StorageValue body) =>
{
    storage[key] = body.Value;
    return Results.NoContent();
});

app.MapGet("/storage/{key}", (string key) =>
{
    if (!storage.TryGetValue(key, out var value))
        return Results.NotFound();
    return Results.Ok(new StorageValue(value));
});

app.Run();

record StorageValue(string Value);
