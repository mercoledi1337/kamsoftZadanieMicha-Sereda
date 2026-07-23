using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapPost("/api/v1/parse-content", (payload r) =>
{
    if (r.type == contentTypes.CSV)
        return Results.Ok();
    else if (r.type == contentTypes.INTERNAL_JSON)
        return Results.Ok();
    else return TypedResults.BadRequest();
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
