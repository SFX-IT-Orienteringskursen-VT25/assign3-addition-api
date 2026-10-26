var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

int? storedValue = null;

// replaces localStorage.setItem
app.MapPost("/api/value", (ValueDto dto) =>
{
    storedValue = dto.Value;
    return Results.Ok(dto);
});

// replaces localStorage.getItem
app.MapGet("/api/value", () =>
    storedValue is null
        ? Results.NotFound()
        : Results.Ok(new ValueDto(storedValue.Value)));

app.Run();

record ValueDto(int Value);