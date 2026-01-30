using Microsoft.AspNetCore.Mvc;

namespace SmartHome.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// GET /health - Health check endpoint
    /// </summary>
    [HttpGet]
    public ActionResult GetHealth()
    {
        return Ok(new { status = "ok" });
    }
}
