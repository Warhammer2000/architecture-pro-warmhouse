var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction()) // Allow Swagger in Prod for review
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var devices = new List<Device>();

app.MapGet("/devices", () => devices);

app.MapPost("/devices", (Device device) =>
{
    device = device with { Id = Guid.NewGuid() };
    devices.Add(device);
    return Results.Created($"/devices/{device.Id}", device);
});

app.MapPost("/devices/{id}/commands", (Guid id, Command command) =>
{
    var device = devices.FirstOrDefault(d => d.Id == id);
    if (device is null) return Results.NotFound();
    
    Console.WriteLine($"Executing command {command.Name} on device {id}");
    return Results.Ok(new { Status = "Executed" });
});

app.Run();

record Device(Guid Id, string Name, string Type, string Location);
record Command(string Name, object Payload);
