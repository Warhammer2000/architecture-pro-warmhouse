# Smart Home Sensor Management API

## Prerequisites

- Docker and Docker Compose
- .NET 10 SDK (for local development)

## Getting Started

### Option 1: Using Docker Compose (Recommended)

The easiest way to start the application is to use Docker Compose:

```bash
./init.sh
```

This script will:

1. Build and start the PostgreSQL and application containers
2. Wait for the services to be ready
3. Display information about how to access the API

Alternatively, you can run Docker Compose directly:

```bash
docker-compose up -d
```

The API will be available at http://localhost:8080

Swagger UI documentation is available at http://localhost:8080/swagger (in Development mode)

### Option 2: Manual setup

If you prefer to run the application without Docker:

1. Start the PostgreSQL database:

```bash
docker-compose up -d postgres
```

2. Build and run the application:

```bash
dotnet build SmartHome.slnx
dotnet run --project SmartHome.Api
```

## API Testing

A Postman collection is provided for testing the API. Import the `smarthome-api.postman_collection.json` file into Postman to get started.

## API Endpoints

- `GET /health` - Health check
- `GET /api/v1/sensors` - Get all sensors
- `GET /api/v1/sensors/{id}` - Get a specific sensor
- `GET /api/v1/sensors/temperature/{location}` - Get temperature by location
- `POST /api/v1/sensors` - Create a new sensor
- `PUT /api/v1/sensors/{id}` - Update a sensor
- `DELETE /api/v1/sensors/{id}` - Delete a sensor
- `PATCH /api/v1/sensors/{id}/value` - Update a sensor's value and status

## Project Structure

```
SmartHome.Api/
├── Controllers/
│   ├── HealthController.cs      # Health check endpoint
│   └── SensorsController.cs     # Sensor CRUD operations
├── Data/
│   └── ApplicationDbContext.cs  # Entity Framework DbContext
├── Models/
│   ├── Sensor.cs                # Sensor entity
│   ├── SensorDtos.cs            # DTOs for create/update
│   └── TemperatureResponse.cs   # Temperature API response model
├── Services/
│   └── TemperatureService.cs    # External temperature API client
├── Program.cs                   # Application entry point
├── appsettings.json             # Configuration
├── Dockerfile                   # Docker build configuration
├── init.sql                     # Database initialization script
└── SmartHome.Api.csproj         # Project file
```

## Technology Stack

- **Framework**: ASP.NET Core (.NET 10)
- **Database**: PostgreSQL with Entity Framework Core
- **API Documentation**: Swagger/OpenAPI
- **Containerization**: Docker
