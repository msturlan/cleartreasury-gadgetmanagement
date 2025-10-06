using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClearTreasury.GadgetManagement.Api.Data.Configurations;

internal class CategoryConfiguration : BaseEntityConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(AppConstants.CategoryNameMaxLength);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
