namespace ClearTreasury.GadgetManagement.Api.Models;

public class Category : IEntityWithId<int>
{
    public Category(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }

    public string Name { get; private set; }
}
