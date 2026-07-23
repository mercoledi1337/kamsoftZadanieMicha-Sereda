using CsvHelper;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

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
        var data = records
                .Select(rec => ((IDictionary<string, object>)rec)
                    .ToDictionary(kv => kv.Key, kv => (object?)kv.Value))
                .ToList();
        return Results.Ok(new ParseResult(true, data.Count, data));
    }
    else if (r.type == contentTypes.INTERNAL_JSON)
    {
        var json = JsonSerializer.Deserialize<testJsonModel>(resBase64);
        var asDict = JsonSerializer.Deserialize<Dictionary<string, object?>>(
                JsonSerializer.Serialize(json));

        var data = new List<Dictionary<string, object?>> { asDict! };

        return Results.Ok(new ParseResult(true, data.Count, data));

    }
    else return Results.BadRequest();
})
.WithName("parse-content");

app.Run();