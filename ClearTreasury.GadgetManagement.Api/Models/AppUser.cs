using Microsoft.AspNetCore.Identity;

namespace ClearTreasury.GadgetManagement.Api.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = String.Empty;
}
