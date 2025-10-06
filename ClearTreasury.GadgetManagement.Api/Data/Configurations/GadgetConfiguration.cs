using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClearTreasury.GadgetManagement.Api.Data.Configurations;

internal class GadgetConfiguration : BaseEntityConfiguration<Gadget>
{
    public override void Configure(EntityTypeBuilder<Gadget> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(AppConstants.GadgetNameMaxLength);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasIndex(x => x.DateCreated);
    }
}
