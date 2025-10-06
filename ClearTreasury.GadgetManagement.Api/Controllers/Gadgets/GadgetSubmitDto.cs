namespace ClearTreasury.GadgetManagement.Api.Controllers.Gadgets;

public record GadgetSubmitDto
{
    public string Name { get; init; } = String.Empty;

    public int Quantity { get; init; }

    public int[] CategoryIds { get; init; } = [];
}
