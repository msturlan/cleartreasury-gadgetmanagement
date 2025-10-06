using Microsoft.AspNetCore.Mvc;

namespace ClearTreasury.GadgetManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class AppBaseController : ControllerBase
{
    protected CancellationToken AbortToken => HttpContext.RequestAborted;

    protected ObjectResult ConflictProblem(string? details = default)
    {
        return Problem(statusCode: StatusCodes.Status409Conflict, detail: details);
    }
}
