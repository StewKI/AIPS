using Microsoft.AspNetCore.Mvc;

namespace AipsWebApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet("/health")]
    public IActionResult GetHealth()
    {
        return Ok();
    }
}