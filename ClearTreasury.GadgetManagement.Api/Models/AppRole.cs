using Microsoft.AspNetCore.Identity;

namespace ClearTreasury.GadgetManagement.Api.Models;

public class AppRole : IdentityRole
{
    public string Description { get; set; } = String.Empty;
}
