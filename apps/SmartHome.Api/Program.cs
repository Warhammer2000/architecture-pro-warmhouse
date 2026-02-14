using Microsoft.EntityFrameworkCore;
using SmartHome.Api.Data;
using SmartHome.Api.Services;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? "Host=localhost;Port=5432;Database=smarthome;Username=postgres;Password=postgres";

if (connectionString.StartsWith("postgres://"))
{
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');
    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]}";
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var temperatureApiUrl = builder.Configuration.GetValue<string>("TemperatureApi:BaseUrl") 
    ?? Environment.GetEnvironmentVariable("TEMPERATURE_API_URL")
    ?? "http://temperature-api:8081";

builder.Services.AddHttpClient<ITemperatureService, TemperatureService>(client =>
{
    client.BaseAddress = new Uri(temperatureApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SmartHome API",
        Version = "v1",
        Description = "API for Smart Home sensor management"
    });
});

var app = builder.Build();

// Auto-create database and tables on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        db.Database.EnsureCreated();
        app.Logger.LogInformation("Database initialized successfully");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error initializing database");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
if (!port.StartsWith(":"))
{
    port = port.TrimStart(':');
}

app.Logger.LogInformation("Server starting on port {Port}", port);
app.Logger.LogInformation("Temperature service initialized with API URL: {Url}", temperatureApiUrl);

app.Run($"http://0.0.0.0:{port}");
