using System.Text.Json.Serialization;

namespace SmartHome.Api.Models;

/// <summary>
/// DTO for updating an existing sensor
/// </summary>
public class SensorUpdateDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("value")]
    public double? Value { get; set; }

    [JsonPropertyName("unit")]
    public string? Unit { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}