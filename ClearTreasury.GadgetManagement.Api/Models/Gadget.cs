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

    public DateTime? DateModified { get; private set; }

    public byte[] RowVersion { get; private set; }

    public string NameGrams { get; private set; } = String.Empty;

    public IReadOnlyList<Category> Categories => _categories.AsReadOnly();

    public void IncreaseStock()
    {
        Quantity++;
        MarkAsUpdated();
    }

    public void DecreaseStock()
    {
        if (Quantity > 0)
        {
            Quantity--;
            MarkAsUpdated();
        }
    }

    public void Update(string name, int quantity)
    {
        if (name != Name || quantity != Quantity)
        {
            Name = Guard.NotNullOrWhitespace(name);
            Quantity = quantity;
            MarkAsUpdated();
        }
    }

    public void SetCategories(Category[] categories)
    {
        _categories.AddRange(categories);
    }

    public void UpdateCategories(Category[] categories)
    {
        var categoriesToAdd = new List<Category>();
        var categoriesToRemove = new List<Category>();

        foreach (var pendingCategory in categories)
        {
            if (Categories.Any(x => x.Id == pendingCategory.Id))
            {
                continue;
            }

            categoriesToAdd.Add(pendingCategory);
        }

        foreach (var currentCategory in Categories)
        {
            if (!categories.Any(x => x.Id == currentCategory.Id))
            {
                categoriesToRemove.Add(currentCategory);
            }
        }

        foreach (var c in categoriesToRemove)
        {
            _categories.Remove(c);
        }

        _categories.AddRange(categoriesToAdd);

        if (categoriesToRemove.Count > 0 || categoriesToAdd.Count > 0)
        {
            MarkAsUpdated();
        }
    }

    private void MarkAsUpdated()
    {
        DateModified = DateTime.UtcNow;
    }
}
