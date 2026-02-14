FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SmartHome.Telemetry.csproj", "."]
RUN dotnet restore "./SmartHome.Telemetry.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SmartHome.Telemetry.csproj" -c Release -o /app/build
RUN dotnet publish "SmartHome.Telemetry.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "SmartHome.Telemetry.dll"]
