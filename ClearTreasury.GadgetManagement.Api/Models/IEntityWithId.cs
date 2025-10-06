namespace ClearTreasury.GadgetManagement.Api.Models;

public interface IEntityWithId<T> where T : IComparable
{
    T Id { get; }
}
