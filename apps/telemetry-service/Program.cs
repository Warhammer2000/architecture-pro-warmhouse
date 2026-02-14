var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/telemetry", (TelemetryData data) =>
{
    Console.WriteLine($"Received telemetry for device {data.DeviceId}: {data.Value} {data.Unit}");
    return Results.Ok();
});

app.MapGet("/health", () => "Telemetry Service is healthy");

app.Run();

record TelemetryData(string DeviceId, double Value, string Unit, DateTimeOffset Timestamp);
