using SkymeyLib.Models.Users.Login;
using SkymeyUserService.Data;

namespace SkymeyUserService.Interfaces.Users.Auth
{
    public interface IUserService
    {
        Task <bool> IsValidUserInformation(LoginModel model, ApplicationContext db);

        string GetUserDetails();
    }
}
