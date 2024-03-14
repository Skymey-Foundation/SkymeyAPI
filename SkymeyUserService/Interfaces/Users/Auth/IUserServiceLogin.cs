using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Register;
using SkymeyUserService.Interfaces.Users.TokenService;
using SkymeyUserService.Services.User;

namespace SkymeyUserService.Interfaces.Users.Auth
{
    public interface IUserServiceLogin
    {
        void UserServiceLoginInit(ApplicationContext db, ITokenService tokenService);
        Task <UserResponse> IsValidUserInformation(LoginModel model, ApplicationContext db);
        Task<UserResponse> Login(LoginModel loginModel);
    }
}
