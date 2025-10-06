using ClearTreasury.GadgetManagement.Api.Extensions;
using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClearTreasury.GadgetManagement.Api.Data;

public static class DbSeeding
{
    public static async Task MigrateAndSeed(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        
        await dbContext.Database.MigrateAsync();
        await SeedCategories(dbContext);
        await SeedRoles(serviceProvider.GetRequiredService<RoleManager<IdentityRole>>());
        await SeedUsers(serviceProvider.GetRequiredService<UserManager<AppUser>>());
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        var roleNames = typeof(AppStaticRoles).GetNonEmptyStringConsts();

        foreach (var name in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(name))
            {
                await roleManager.CreateAsync(new IdentityRole(name));
            }
        }
    }

    private static async Task SeedUsers(UserManager<AppUser> userManager)
    {
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

    private static async Task SeedCategories(DbContext dbContext)
    {
        var categorySet = dbContext.Set<Category>();

        foreach (var name in typeof(CategoryNames).GetNonEmptyStringConsts())
        {
            if (!await categorySet.AnyAsync(x => x.Name == name))
            {
                categorySet.Add(new Category(name));
            }
        }

        await dbContext.SaveChangesAsync();
    }
}
