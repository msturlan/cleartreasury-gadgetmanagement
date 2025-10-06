using System.ComponentModel.DataAnnotations;
using ClearTreasury.GadgetManagement.Api.Models;

namespace ClearTreasury.GadgetManagement.Api.Controllers.Gadgets;

public record GadgetSearchDto
{
    [Range(0, Int32.MaxValue)]
    public int PageIndex { get; init; }

    [Range(10, 1000)]
    public int PageSize { get; init; } = 10;

    [MinLength(AppConstants.GadgetNameFilterMinLength)]
    [MaxLength(16)]
    public string? NameFilter { get; init; }

    public DateTime? DateFromFilter { get; init; }

    public DateTime? DateToFilter { get; init; }
}
