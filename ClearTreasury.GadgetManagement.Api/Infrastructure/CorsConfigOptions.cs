using System.ComponentModel.DataAnnotations;

namespace ClearTreasury.GadgetManagement.Api.Infrastructure;

public record CorsConfigOptions
{
    [Required]
    public string[] Origins { get; set; } = [];
}
