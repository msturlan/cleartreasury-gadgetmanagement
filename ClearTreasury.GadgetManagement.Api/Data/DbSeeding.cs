using System.Reflection;
using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClearTreasury.GadgetManagement.Api.Data;

public static class DbSeeding
{
    public static async Task SeedAsync(DbContext context,
        bool storeMgmtOperation, bool infrastructureDataOnly, CancellationToken ct)
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

            var role = await context.Set<AppRole>()
                .FirstOrDefaultAsync(x => x.Name == name, ct);

            if (role is null)
            {
                context.Set<AppRole>().Add(new AppRole(name));
            }
        }

        await context.SaveChangesAsync(ct);

        if (!infrastructureDataOnly)
        {
            // generate demo users here
        }
    }
}
