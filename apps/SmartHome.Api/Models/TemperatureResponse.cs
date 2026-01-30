using System.Text.Json.Serialization;

namespace SmartHome.Api.Models;

/// <summary>
/// Represents the response from the temperature API
/// </summary>
public class TemperatureResponse
{
    [JsonPropertyName("value")]
    public double Value { get; set; }

    [JsonPropertyName("unit")]
    public string Unit { get; set; } = string.Empty;

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("sensor_id")]
    public string SensorId { get; set; } = string.Empty;

    [JsonPropertyName("sensor_type")]
    public string SensorType { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}