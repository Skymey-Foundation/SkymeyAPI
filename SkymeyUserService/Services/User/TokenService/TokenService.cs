using Microsoft.IdentityModel.Tokens;
using SkymeyUserService.Interfaces.Users.TokenService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SkymeyUserService.Services.User.TokenService
{
    public class TokenService : ITokenService, IDisposable
    {
        private IConfiguration _configuration;
        private byte[]? _key;
        private string? _Issuer;
        private string? _Audience;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            _Issuer = _configuration["Jwt:Issuer"];
            _Audience = _configuration["Jwt:Audience"];
        }

        public string GenerateJwtToken(string userName)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userName) }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                Issuer = _Issuer,
                Audience = _Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            byte[]? randomNumber = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_key),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public void Dispose()
        {

        }
    }
}
