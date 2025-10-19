using AdditionApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//builder.Services.AddControllers(); // For API later

var app = builder.Build();

// Use default files and static files (serves index.html automatically)
app.UseDefaultFiles(); // looks for index.html by default
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Optional: Use HTTPS redirection
app.UseHttpsRedirection();


// Map a default API route for testing
app.MapGet("/api/hello", () =>
{
    return "Hello World!";
});

// -----------------API for localstorage.setItem and getItem-----------------
// GET api / numbers->returns all numbers and sum
// In-memory storage for numbers
var numbersData = new Number();

// GET api/numbers -> returns all numbers and sum
app.MapGet("/api/numbers", () =>
{
    if (!numbersData.Numbers.Any())
        return Results.NotFound(new { message = "No numbers persisted." });

    return Results.Ok(new
    {
        numbers = numbersData.Numbers,
        sum = numbersData.Sum
    });
});

// POST api/numbers -> add new numbers
app.MapPost("/api/numbers", (List<int> newNumbers) =>
{
    if (newNumbers == null || !newNumbers.Any())
        return Results.BadRequest(new { message = "No numbers provided." });

    numbersData.Numbers.AddRange(newNumbers);

    return Results.Created("/api/numbers", new
    {
        numbers = numbersData.Numbers,
        sum = numbersData.Sum
    });
});


// Run the app
app.Run();

