namespace ClearTreasury.GadgetManagement.Api.Controllers.Gadgets;

public record GadgetDeleteResultDto(Guid Id, bool Deleted, string? Reason)
{
    public static GadgetDeleteResultDto Ok(Guid id) => new(id, true, null);

    public static GadgetDeleteResultDto Fail(Guid id, string reason) => new(id, false, reason);
}
