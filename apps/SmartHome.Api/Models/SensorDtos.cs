using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartHome.Api.Models;

/// <summary>
/// DTO for updating sensor value
/// </summary>
public class SensorValueUpdateDto
{
    [Required]
    [JsonPropertyName("value")]
    public double Value { get; set; }

    [Required]
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}
