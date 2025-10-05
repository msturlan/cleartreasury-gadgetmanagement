using Microsoft.AspNetCore.Identity;

namespace ClearTreasury.GadgetManagement.Api.Models;

public class AppRole : IdentityRole
{
    public AppRole() : base()
    {
    }

    public AppRole(string roleName) : base(roleName)
    {
    }
}
