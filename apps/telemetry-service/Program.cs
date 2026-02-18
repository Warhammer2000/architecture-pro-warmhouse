var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/telemetry", (TelemetryData data) =>
{
    Console.WriteLine($"Received telemetry for device {data.DeviceId}: {data.Value} {data.Unit}");
    return Results.Ok();
});

app.MapGet("/health", () => "Telemetry Service is healthy");

app.Run();

record TelemetryData(string DeviceId, double Value, string Unit, DateTimeOffset Timestamp);
