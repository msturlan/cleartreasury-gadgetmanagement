using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClearTreasury.GadgetManagement.Api.Infrastructure;

public record JwtAuthOptions
{
    [Required]
    public string Issuer { get; set; } = String.Empty;

    [Required]
    public string Audience {  get; set; } = String.Empty;

    [Required]
    public string SecretKey { get; set; } = String.Empty;

    [Range(1, 1440)]
    public int ExpiresInMins { get; set; }

    public byte[] GetSecretKeyBytes() => Encoding.UTF8.GetBytes(SecretKey);
}
