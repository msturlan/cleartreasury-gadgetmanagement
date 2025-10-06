using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace ClearTreasury.GadgetManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class AppBaseController : ControllerBase
{
    protected CancellationToken AbortToken => HttpContext.RequestAborted;

    protected ObjectResult CreateProblem(int statusCode)
    {
        return Problem(
            statusCode: statusCode,
            title: ReasonPhrases.GetReasonPhrase(statusCode),
            extensions: new Dictionary<string, object?>()
            {
                ["traceId"] = HttpContext.TraceIdentifier
            });
    }
}
