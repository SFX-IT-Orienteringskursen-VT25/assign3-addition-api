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

var storage = new Dictionary<string, string>();


app.MapPost("/storage", ([FromBody] StorageItem item) =>
{
    if (string.IsNullOrEmpty(item.Key))
    {
        return Results.BadRequest("Key is required");
    }

    storage[item.Key] = item.Value;

    return Results.Created($"/storage/{item.Key}", item);
});

app.MapGet("/storage/{key}", (string key) =>
{
    if (!storage.ContainsKey(key))
    {
        return Results.NotFound();
    }

    return Results.Ok(new StorageItem
    {
        Key = key,
        Value = storage[key]
    });
});

app.Run();

public class StorageItem
{
    public string Key { get; set; }
    public string Value { get; set; }
}


