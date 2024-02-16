using SkymeyLib.Models.Users.Login;

namespace SkymeyUserService.Interfaces.Users.Auth
{
    public interface IUserService
    {
        bool IsValidUserInformation(LoginModel model);

        string GetUserDetails();
    }
}
