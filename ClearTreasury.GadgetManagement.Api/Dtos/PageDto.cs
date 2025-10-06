namespace ClearTreasury.GadgetManagement.Api.Dtos;

public record PageDto<T>
{
    private readonly List<T> _values = [];

    public PageDto(int count)
    {
        Count = count;
    }

    public int Count { get; private set; }

    public IReadOnlyCollection<T> Items => _values.AsReadOnly();

    public void Add(T item)
    {
        _values.Add(item);
    }
}
