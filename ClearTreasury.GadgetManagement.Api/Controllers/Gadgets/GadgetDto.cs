namespace ClearTreasury.GadgetManagement.Api.Controllers.Gadgets;

public record GadgetDto
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required int Quantity { get; set; }

    public required DateTime DateCreated { get; set; }

    public required DateTime DateModified { get; set; }

    public required byte[] RowVersion { get; set; }

    public required GadgetCategoryDto[] Categories { get; set; }

    public record GadgetCategoryDto(int Id, string Name);
}
