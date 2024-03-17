using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Register;
using SkymeyUserService.Interfaces.Users.TokenService;
using SkymeyUserService.Services.User;

namespace SkymeyUserService.Interfaces.Users.Login
{
    public interface IUserServiceLogin
    {
        Task <UserResponse> IsValidUserInformation(LoginModel model);
        Task<UserResponse> Login(LoginModel loginModel);
    }
}
