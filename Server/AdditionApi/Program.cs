using Microsoft.AspNetCore.Builder;
using System.Collections.Concurrent;


var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllers();
builder.Services.AddOpenApi();
var app = builder.Build();

ConcurrentDictionary<string, string> items = new ConcurrentDictionary<string, string>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () =>
{
    return "Hello World!";
});

app.MapGet("/getItems", () =>
{
    // Sort with numeric ordering for keys that are numbers
    var sortedItems = items
        .OrderBy(e => 
        {
            
            if (int.TryParse(e.Key, out int intValue))
                return intValue;
            
            
            if (double.TryParse(e.Key, out double doubleValue))
                return (int)doubleValue; 
            
            
            return int.MaxValue;
        })
        .ThenBy(e => e.Key) // Secondary alphabetical sort for non-numeric keys
        .ToDictionary(e => e.Key, e => e.Value);

    return Results.Ok(sortedItems);
});

app.MapPost("/addItems", (Item[] itemsArray) =>
{
    var results = new List<Item>();
    foreach (var item in itemsArray)
    {
        if (string.IsNullOrEmpty(item.Id) || item.Value == null)
        {
            return Results.BadRequest($"Invalid item: Id and Value are required for all items");
        }
        
        items[item.Id] = item.Value;
        results.Add(item);
    }
    return Results.Created($"/getItems", results);
});

app.Run();

public class Item
{
    public string Id { get; set; } =string.Empty;
    public string Value { get; set; } = string.Empty;
}