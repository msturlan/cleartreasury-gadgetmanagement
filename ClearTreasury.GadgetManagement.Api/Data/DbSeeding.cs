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
        await SeedGadgets(dbContext);

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
            FullName = "Cindy User",
            UserName = "user@ct.com",
            Email = "user@ct.com",
            EmailConfirmed = true,
            LockoutEnabled = false
        };
        var manager = new AppUser()
        {
            FullName = "John Manager",
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
    
    private static async Task SeedGadgets(DbContext dbContext)
    {
        var gadgetSet= dbContext.Set<Gadget>();

        var categories = await dbContext
            .Set<Category>()
            .ToDictionaryAsync(x => x.Name, x => x);

        var gadget1 = new Gadget("Samsung Galaxy S24", 5);
        gadget1.SetCategories([categories[CategoryNames.Communication]]);

        var gadget2 = new Gadget("Apple Watch III", 2);
        gadget2.SetCategories([categories[CategoryNames.Wireless]]);

        if (!await gadgetSet.AnyAsync(x => x.Name == gadget1.Name))
        {
            gadgetSet.Add(gadget1);
        }
        if (!await gadgetSet.AnyAsync(x => x.Name == gadget2.Name))
        {
            gadgetSet.Add(gadget2);
        }

        await dbContext.SaveChangesAsync();
    }
}
