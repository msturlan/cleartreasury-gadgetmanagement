using System.Reflection;
using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace ClearTreasury.GadgetManagement.Api.Data;

public static class DbSeeding
{
    public static async Task SeedAsync(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        var roleNames = typeof(AppStaticRoles)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(x => x.IsLiteral && !x.IsInitOnly)
            .Select(x => x.GetValue(default) as string)
            .ToArray();

        foreach (var name in roleNames)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            if (!await roleManager.RoleExistsAsync(name))
            {
                await roleManager.CreateAsync(new IdentityRole(name));
            }
        }

        var user = new AppUser()
        {
            UserName = "user@ct.com",
            Email = "user@ct.com",
            EmailConfirmed = true,
            LockoutEnabled = false
        };
        var manager = new AppUser()
        {
            UserName = "manager@ct.com",
            Email = "manager@ct.com",
            EmailConfirmed = true,
            LockoutEnabled = false
        };
        
        userManager.PasswordValidators.Clear();
        if (await userManager.FindByEmailAsync(user.Email) is null)
        {
            await userManager.CreateAsync(user, "user");
            await userManager.AddToRoleAsync(user, AppStaticRoles.User);
        }
        if (await userManager.FindByEmailAsync(manager.Email) is null)
        {
            await userManager.CreateAsync(manager, "manager");
            await userManager.AddToRoleAsync(manager, AppStaticRoles.Manager);
        }
    }
}
