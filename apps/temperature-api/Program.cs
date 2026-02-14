using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/temperature", (string? location, string? sensorId) =>
{

    if (string.IsNullOrEmpty(location))
    {
        location = sensorId switch
        {
            "1" => "Living Room",
            "2" => "Bedroom",
            "3" => "Kitchen",
            _ => "Unknown"
        };
    }


    if (string.IsNullOrEmpty(sensorId))
    {
        sensorId = location switch
        {
            "Living Room" => "1",
            "Bedroom" => "2",
            "Kitchen" => "3",
            _ => "0"
        };
    }


    var temperature = 15.0 + Random.Shared.NextDouble() * (30.0 - 15.0);

    return new TemperatureResponse
    (
        Location: location,
        SensorId: sensorId,
        Temperature: temperature,
        Unit: "Celsius"
    );
});

app.Run();

record TemperatureResponse(
    [property: JsonPropertyName("location")] string Location,
    [property: JsonPropertyName("sensorId")] string SensorId,
    [property: JsonPropertyName("temperature")] double Temperature,
    [property: JsonPropertyName("unit")] string Unit
);
