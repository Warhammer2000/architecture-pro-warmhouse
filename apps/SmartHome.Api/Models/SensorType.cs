using System.Text.Json.Serialization;

namespace SmartHome.Api.Models;

/// <summary>
/// Represents the type of sensor
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SensorType
{
    [JsonPropertyName("temperature")]
    Temperature
}
