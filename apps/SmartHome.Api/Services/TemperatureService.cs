using System.Net.Http.Json;
using SmartHome.Api.Models;

namespace SmartHome.Api.Services;

/// <summary>
/// Service for fetching temperature data from external API
/// </summary>
public interface ITemperatureService
{
    Task<TemperatureResponse?> GetTemperatureAsync(string location);
    Task<TemperatureResponse?> GetTemperatureByIdAsync(string sensorId);
}

/// <summary>
/// Implementation of temperature service
/// </summary>
public class TemperatureService : ITemperatureService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TemperatureService> _logger;

    public TemperatureService(HttpClient httpClient, ILogger<TemperatureService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Fetches temperature data for a specific location
    /// </summary>
    public async Task<TemperatureResponse?> GetTemperatureAsync(string location)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/temperature?location={Uri.EscapeDataString(location)}");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch temperature data for location {Location}. Status: {StatusCode}", 
                    location, response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TemperatureResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching temperature data for location {Location}", location);
            return null;
        }
    }

    /// <summary>
    /// Fetches temperature data for a specific sensor ID
    /// </summary>
    public async Task<TemperatureResponse?> GetTemperatureByIdAsync(string sensorId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/temperature/{sensorId}");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch temperature data for sensor {SensorId}. Status: {StatusCode}", 
                    sensorId, response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TemperatureResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching temperature data for sensor {SensorId}", sensorId);
            return null;
        }
    }
}