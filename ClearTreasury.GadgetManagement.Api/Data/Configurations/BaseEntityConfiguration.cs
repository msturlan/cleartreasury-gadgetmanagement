using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClearTreasury.GadgetManagement.Api.Data.Configurations;

internal abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
    where T : class
{
    protected virtual bool AutoGenerateKeyValue => true;

    protected virtual string? Schema => DbSchemas.Inventory;

    protected virtual string? TableName => null;

    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        if (Schema is not null)
        {
            builder.Metadata.SetSchema(Schema);
        }

        if (TableName is not null)
        {
            builder.Metadata.SetTableName(TableName);
        }

        Type entityType = typeof(T);

        bool hasId = entityType.GetInterfaces().Any(
            i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityWithId<>));

        if (hasId)
        {
            var keyPropName = nameof(IEntityWithId<>.Id);

            builder.HasKey(keyPropName);

            builder.Property(keyPropName)
                .HasColumnOrder(0);

            if (AutoGenerateKeyValue)
            {
                builder.Property(keyPropName)
                    .ValueGeneratedOnAdd();
            }
        }

        if (typeof(IVersionedEntity).IsAssignableFrom(entityType))
        {
            builder
                .Property(nameof(IVersionedEntity.RowVersion))
                .IsRowVersion();
        }
    }
}
