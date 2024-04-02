using SkymeyLib.Models.Users.Login;

namespace SkymeyLib.Models.Users
{
    public interface IUserResponseJWT
    {
        public AuthenticatedResponse AuthenticatedResponses { get; set; }
    }
}
