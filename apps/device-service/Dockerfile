FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SmartHome.Device.csproj", "."]
RUN dotnet restore "./SmartHome.Device.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SmartHome.Device.csproj" -c Release -o /app/build
RUN dotnet publish "SmartHome.Device.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "SmartHome.Device.dll"]
