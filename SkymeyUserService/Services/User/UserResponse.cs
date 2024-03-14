using SkymeyLib.Models.Users.Login;
using SkymeyUserService.Interfaces.Users.Register;

namespace SkymeyUserService.Services.User
{
    public class UserResponse : IUserResponse
    {
        public bool ResponseType { get; set; }
        public string Response { get; set; }
        public AuthenticatedResponse AuthenticatedResponses { get; set; }
    }
}
