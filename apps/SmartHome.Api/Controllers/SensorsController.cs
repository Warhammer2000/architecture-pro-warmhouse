using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHome.Api.Data;
using SmartHome.Api.Models;
using SmartHome.Api.Services;

namespace SmartHome.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SensorsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITemperatureService _temperatureService;
    private readonly ILogger<SensorsController> _logger;

    public SensorsController(
        ApplicationDbContext context,
        ITemperatureService temperatureService,
        ILogger<SensorsController> logger)
    {
        _context = context;
        _temperatureService = temperatureService;
        _logger = logger;
    }

    /// <summary>
    /// GET /api/v1/sensors - Get all sensors
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sensor>>> GetSensors()
    {
        var sensors = await _context.Sensors.OrderBy(s => s.Id).ToListAsync();

        foreach (var sensor in sensors.Where(s => s.Type == "temperature"))
        {
            var tempData = await _temperatureService.GetTemperatureByIdAsync(sensor.Id.ToString());
            if (tempData != null)
            {
                sensor.Value = tempData.Value;
                sensor.Status = tempData.Status;
                sensor.LastUpdated = tempData.Timestamp;
                _logger.LogInformation("Updated temperature data for sensor {SensorId} from external API", sensor.Id);
            }
            else
            {
                _logger.LogWarning("Failed to fetch temperature data for sensor {SensorId}", sensor.Id);
            }
        }

        return Ok(sensors);
    }

    /// <summary>
    /// GET /api/v1/sensors/{id} - Get sensor by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Sensor>> GetSensorById(int id)
    {
        var sensor = await _context.Sensors.FindAsync(id);

        if (sensor == null)
        {
            return NotFound(new { error = "Sensor not found" });
        }

        if (sensor.Type == SensorType.Temperature.ToString())
        {
            var tempData = await _temperatureService.GetTemperatureByIdAsync(sensor.Id.ToString());
            if (tempData != null)
            {
                sensor.Value = tempData.Value;
                sensor.Status = tempData.Status;
                sensor.LastUpdated = tempData.Timestamp;
                _logger.LogInformation("Updated temperature data for sensor {SensorId} from external API", sensor.Id);
            }
            else
            {
                _logger.LogWarning("Failed to fetch temperature data for sensor {SensorId}", sensor.Id);
            }
        }

        return Ok(sensor);
    }

    /// <summary>
    /// GET /api/v1/sensors/temperature/{location} - Get temperature by location
    /// </summary>
    [HttpGet("temperature/{location}")]
    public async Task<ActionResult> GetTemperatureByLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            return BadRequest(new { error = "Location is required" });
        }

        var tempData = await _temperatureService.GetTemperatureAsync(location);

        if (tempData == null)
        {
            return StatusCode(500, new { error = "Failed to fetch temperature data" });
        }

        return Ok(new
        {
            location = tempData.Location,
            value = tempData.Value,
            unit = tempData.Unit,
            status = tempData.Status,
            timestamp = tempData.Timestamp,
            description = tempData.Description
        });
    }

    /// <summary>
    /// POST /api/v1/sensors - Create a new sensor
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Sensor>> CreateSensor([FromBody] SensorCreateDto sensorCreate)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var now = DateTime.UtcNow;
        var sensor = new Sensor
        {
            Name = sensorCreate.Name,
            Type = sensorCreate.Type,
            Location = sensorCreate.Location,
            Unit = sensorCreate.Unit,
            Status = "inactive",
            LastUpdated = now,
            CreatedAt = now
        };

        _context.Sensors.Add(sensor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSensorById), new { id = sensor.Id }, sensor);
    }

    /// <summary>
    /// PUT /api/v1/sensors/{id} - Update an existing sensor
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<Sensor>> UpdateSensor(int id, [FromBody] SensorUpdateDto sensorUpdate)
    {
        var sensor = await _context.Sensors.FindAsync(id);

        if (sensor == null)
        {
            return NotFound(new { error = "Sensor not found" });
        }

        if (!string.IsNullOrEmpty(sensorUpdate.Name))
            sensor.Name = sensorUpdate.Name;

        if (!string.IsNullOrEmpty(sensorUpdate.Type))
            sensor.Type = sensorUpdate.Type;

        if (!string.IsNullOrEmpty(sensorUpdate.Location))
            sensor.Location = sensorUpdate.Location;

        if (sensorUpdate.Value.HasValue)
            sensor.Value = sensorUpdate.Value.Value;

        if (!string.IsNullOrEmpty(sensorUpdate.Unit))
            sensor.Unit = sensorUpdate.Unit;

        if (!string.IsNullOrEmpty(sensorUpdate.Status))
            sensor.Status = sensorUpdate.Status;

        sensor.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(sensor);
    }

    /// <summary>
    /// DELETE /api/v1/sensors/{id} - Delete a sensor
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSensor(int id)
    {
        var sensor = await _context.Sensors.FindAsync(id);

        if (sensor == null)
        {
            return NotFound(new { error = "Sensor not found" });
        }

        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Sensor deleted successfully" });
    }

    /// <summary>
    /// PATCH /api/v1/sensors/{id}/value - Update sensor value
    /// </summary>
    [HttpPatch("{id}/value")]
    public async Task<ActionResult> UpdateSensorValue(int id, [FromBody] SensorValueUpdateDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var sensor = await _context.Sensors.FindAsync(id);

        if (sensor == null)
        {
            return NotFound(new { error = "Sensor not found" });
        }

        sensor.Value = request.Value;
        sensor.Status = request.Status;
        sensor.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Sensor value updated successfully" });
    }
}