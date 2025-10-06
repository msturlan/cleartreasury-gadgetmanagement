using System.ComponentModel.DataAnnotations;

namespace ClearTreasury.GadgetManagement.Api.Controllers.Identity;

public record LoginRequest(
    [Required] string Email,
    [Required] string Password);
