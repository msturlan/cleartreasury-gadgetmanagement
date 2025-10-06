using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClearTreasury.GadgetManagement.Api.Controllers.Identity;

public class MeController : AppBaseController
{
    [HttpGet]
    [Authorize]
    public ActionResult Get()
    {
        return Ok(User.Claims.ToDictionary(x => x.Type, x => x.Value));
    }
}
