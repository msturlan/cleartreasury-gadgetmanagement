using Microsoft.AspNetCore.Mvc;

namespace ClearTreasury.GadgetManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Status = "Healthy",
                ServerTime = DateTime.UtcNow.ToString("o")
            });
        }
    }
}
