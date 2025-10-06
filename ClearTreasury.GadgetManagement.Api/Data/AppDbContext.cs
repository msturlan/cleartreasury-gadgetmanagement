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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(DbSchemas.Identity);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
