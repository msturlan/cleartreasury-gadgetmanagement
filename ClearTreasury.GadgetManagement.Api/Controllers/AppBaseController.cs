using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace ClearTreasury.GadgetManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class AppBaseController : ControllerBase
{
    protected CancellationToken AbortToken => HttpContext.RequestAborted;

    protected ObjectResult CreateConflictProblem(string? details = default)
    {
        var code = StatusCodes.Status409Conflict;

        return Problem(
            statusCode: code,
            title: ReasonPhrases.GetReasonPhrase(code),
            detail: details);
    }

    protected ObjectResult CreatePreconditionFailedProblem()
    {
        var code = StatusCodes.Status412PreconditionFailed;

        return Problem(
            statusCode: code,
            title: ReasonPhrases.GetReasonPhrase(code),
            detail: "The resource has already been modified by another party",
            instance: HttpContext.Request.Path);
    }
}
