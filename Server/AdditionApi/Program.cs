using AdditionApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

// Storage endpoints
app.MapPut("/storage/{key}", ([FromRoute] string key, [FromBody] StorageValue value) =>
{
    Storage.Set(key, value.Value);
    return Results.Ok(new { success = true });
});

app.MapGet("/storage/{key}", ([FromRoute] string key) =>
{
    var value = Storage.Get(key);
    if (value is null)
        return Results.NotFound();
    return Results.Ok(new StorageValue { Value = value });
});

app.Run();