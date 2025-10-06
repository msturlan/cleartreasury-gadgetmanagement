using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.EntityFrameworkCore;
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
        
        builder
            .Navigation(x => x.Categories)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(x => x.Categories)
            .WithMany();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasIndex(x => x.DateCreated);
    }
}
