using SkymeyLib.Models.Users.Login;
using SkymeyUserService.Interfaces.Users.Auth;

namespace SkymeyUserService.Services.User.Auth
{
    public class UserService : IUserService
    {
        public string GetUserDetails()
        {
            return "ok";
        }

        public bool IsValidUserInformation(LoginModel model)
        {
            if (model.Email.Equals("1") && model.Password.Equals("2")) return true;
            else return false;
        }
    }
}
