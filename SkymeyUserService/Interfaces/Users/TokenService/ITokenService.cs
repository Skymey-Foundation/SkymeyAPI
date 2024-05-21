using System.Security.Claims;

namespace SkymeyUserService.Interfaces.Users.TokenService
{
    public interface ITokenService
    {
        string GenerateJwtToken(string claims, string Role);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string userName);
    }
}
