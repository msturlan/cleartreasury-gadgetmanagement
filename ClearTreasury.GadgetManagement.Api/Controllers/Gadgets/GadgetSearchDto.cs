namespace ClearTreasury.GadgetManagement.Api.Controllers.Gadgets;

public record GadgetSearchDto(
    int PageIndex,
    int PageSize,
    string? NameFilter,
    DateTime? DateFromFilter,
    DateTime? DateToFilter);
