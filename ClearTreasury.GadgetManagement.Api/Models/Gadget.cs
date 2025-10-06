namespace ClearTreasury.GadgetManagement.Api.Models;

public class Gadget : IEntityWithId<Guid>, IVersionedEntity
{
    private readonly List<Category> _categories = [];

    public Gadget(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
        DateCreated = DateTime.UtcNow;
        RowVersion = [];
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public int Quantity { get; private set; }

    public DateTime DateCreated { get; private set; }

    public DateTime DateModified { get; private set; }

    public byte[] RowVersion { get; private set; }

    public IReadOnlyList<Category> Categories => _categories.AsReadOnly();

    public void Update(string name, int quantity)
    {
        if (name != Name || quantity != Quantity)
        {
            Name = Guard.NotNullOrWhitespace(name);
            Quantity = quantity;
            DateModified = DateTime.UtcNow;
        }
    }
}
