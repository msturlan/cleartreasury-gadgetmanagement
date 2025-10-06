using System.ComponentModel.DataAnnotations;
using ClearTreasury.GadgetManagement.Api.Models;

namespace ClearTreasury.GadgetManagement.Api.Controllers.Gadgets;

public record GadgetSubmitDto
{
    [Required]
    [MaxLength(AppConstants.GadgetNameMaxLength)]
    public string Name { get; init; } = String.Empty;

    [Range(0, AppConstants.GadgetQuantityMax)]
    public int Quantity { get; init; }

    public int[] CategoryIds { get; init; } = [];
}
