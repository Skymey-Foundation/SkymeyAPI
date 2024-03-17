using System.Security.Claims;

namespace SkymeyUserService.Interfaces.Users.TokenService
{
    public interface ITokenService
    {
        string GenerateJwtToken(string claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string userName);
    }
}
