namespace ClearTreasury.GadgetManagement.Api.Models;

public interface IVersionedEntity
{
    byte[] RowVersion { get; }
}
