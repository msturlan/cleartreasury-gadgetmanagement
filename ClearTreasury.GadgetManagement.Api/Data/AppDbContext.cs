using System.Data;
using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClearTreasury.GadgetManagement.Api.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public void SetOriginalRowVersion<T>(T entity, byte[] rowVersion)
        where T : class, IVersionedEntity
    {
        Entry(entity).Property(x => x.RowVersion).OriginalValue = rowVersion;
    }

    public async Task<string> PrepareContainsTerm(string term, CancellationToken ct)
    {
        term = await Database
            .SqlQueryRaw<string>("SELECT dbo.fn_Generate3grams({0}) AS value", term)
            .SingleAsync(ct);

        var items = term.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return String.Join(" AND ", items.Select(x => $"\"{x}\""));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(DbSchemas.Identity);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
