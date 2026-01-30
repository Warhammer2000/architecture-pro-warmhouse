using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartHome.Api.Models;

/// <summary>
/// DTO for creating a new sensor
/// </summary>
public class SensorCreateDto
{
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;

    [JsonPropertyName("unit")]
    public string? Unit { get; set; }
}
