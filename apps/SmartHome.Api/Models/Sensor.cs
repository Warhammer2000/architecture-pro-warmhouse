using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SmartHome.Api.Models;

/// <summary>
/// Represents a smart home sensor
/// </summary>
[Table("sensors")]
public class Sensor
{
    [Key]
    [Column("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("type")]
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("location")]
    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;

    [Column("value")]
    [JsonPropertyName("value")]
    public double Value { get; set; }

    [MaxLength(20)]
    [Column("unit")]
    [JsonPropertyName("unit")]
    public string? Unit { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("status")]
    [JsonPropertyName("status")]
    public string Status { get; set; } = "inactive";

    [Column("last_updated")]
    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    [Column("created_at")]
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
