var container = new List<LocalStorage>
{
    new LocalStorage { key = "John", value = "SDE" },
    new LocalStorage { key = "Owilsson", value = "Platform Engineer" }
};

var builder = WebApplication.CreateBuilder(args);

//Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();
app.MapGet("/getData", () => {

    return Results.Ok(localstorage);
});


app.MapPost("/postData", ([FromBody] LocalStorage localstorage, HttpContext context) => {
    container.Add(localstorage)
    return Results.Created(localstorage);
});


app.Run();
