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

// Enable static file serving (from wwwroot)
app.UseStaticFiles();


// Simulate in-memory persistent storage
List<int> storedNumbers = new();

// Serve the HTML file at root
app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/Persisted-addition.html");
});

// GET: Retrieve stored numbers
app.MapGet("/api/numbers", () =>
{
    return Results.Ok(storedNumbers);
});

// POST: Overwrite stored numbers
app.MapPost("/api/numbers", async (HttpContext context) =>
{
    var numbers = await context.Request.ReadFromJsonAsync<List<int>>();
    if (numbers == null)
        return Results.BadRequest("Invalid JSON array");

    storedNumbers = numbers;
    return Results.NoContent();
});



app.Run();