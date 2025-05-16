using AdditionApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Adding Controllers
builder.Services.AddControllers();

// If you need Swagger/OpenAPI, you need to uncomment the lines below
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger for development only (optional)
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseAuthorization();

// Connecting controllers
app.MapControllers();

app.Run();
