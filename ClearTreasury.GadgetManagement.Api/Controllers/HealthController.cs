using Microsoft.AspNetCore.Mvc;

namespace ClearTreasury.GadgetManagement.Api.Controllers
{
    public class HealthController : AppBaseController
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
