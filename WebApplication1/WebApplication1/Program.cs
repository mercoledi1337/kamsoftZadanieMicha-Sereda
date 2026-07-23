
using CsvHelper;
using System.Globalization;
using System.Text.Json.Serialization;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());

    
});
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapPost("/api/v1/parse-content", (payload r) =>
{
    var resBase64 = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(r.content));
    if (r.type == contentTypes.CSV)
    {
        using var reader = new StringReader(resBase64);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<dynamic>().ToList();
        return Results.Ok();
    }
    else if (r.type == contentTypes.INTERNAL_JSON)
        return Results.Ok();
    else return Results.BadRequest();
})
.WithName("parse-content");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
