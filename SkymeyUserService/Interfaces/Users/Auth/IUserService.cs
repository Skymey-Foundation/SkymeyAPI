using SkymeyLib.Models.Users.Login;
using SkymeyUserService.Data;
using SkymeyUserService.Services.User;

namespace SkymeyUserService.Interfaces.Users.Auth
{
    public interface IUserService
    {
        Task <UserResponse> IsValidUserInformation(LoginModel model, ApplicationContext db);

        string GetUserDetails();
    }
}
