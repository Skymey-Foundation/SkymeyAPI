using SkymeyLib.Models.Users.Login;

namespace SkymeyUserService.Interfaces.Users.Register
{
    public interface IUserResponseJWT
    {
        public AuthenticatedResponse AuthenticatedResponses { get; set; }
    }
}
