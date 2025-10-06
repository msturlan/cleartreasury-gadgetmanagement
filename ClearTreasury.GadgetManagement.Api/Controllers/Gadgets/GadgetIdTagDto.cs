namespace ClearTreasury.GadgetManagement.Api.Controllers.Gadgets;

public record GadgetIdTagDto(Guid Id, byte[] ETag);
