using System.Security.Claims;
using ClearTreasury.GadgetManagement.Api.Infrastructure;
using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ClearTreasury.GadgetManagement.Api.Controllers.Identity;

public class LoginController(
    IOptionsSnapshot<JwtAuthOptions> jwtOptions,
    UserManager<AppUser> userManager)
    : AppBaseController
{
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody]LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null ||
            await userManager.IsLockedOutAsync(user) ||
            !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized();
        }

        var roles = await userManager.GetRolesAsync(user);

        var signingKey = new SymmetricSecurityKey(jwtOptions.Value.GetSecretKeyBytes());
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? String.Empty),
            ..roles.Select(x => new Claim(ClaimTypes.Role, x))
        ];

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = jwtOptions.Value.Issuer,
            Audience = jwtOptions.Value.Audience,
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.Value.ExpiresInMins),
            SigningCredentials = credentials
        };

        var tokenHandler = new JsonWebTokenHandler();
        var tokenString = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new
        {
            AccessToken = tokenString
        });
    }
}
