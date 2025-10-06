using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace ClearTreasury.GadgetManagement.Api.Infrastructure;

public class FromIfMatchHeaderAttribute : FromHeaderAttribute
{
    public FromIfMatchHeaderAttribute()
    {
        Name = HeaderNames.IfMatch;
    }
}
