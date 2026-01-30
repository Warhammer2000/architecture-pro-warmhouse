# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy csproj and restore dependencies
COPY SmartHome.Api.csproj .
RUN dotnet restore

# Copy the source code
COPY . .

# Build the application
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

# Copy the published app
COPY --from=build /app/publish .

# Expose the port the app runs on
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Command to run the application
ENTRYPOINT ["dotnet", "SmartHome.Api.dll"]
