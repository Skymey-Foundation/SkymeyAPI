using System.Security.Claims;

namespace SkymeyUserService.Interfaces.Users.TokenService
{
    public interface ITokenService
    {
        IConfiguration configuration(IConfiguration _config);
        string GenerateJwtToken(string claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string userName);
    }
}
