using Microsoft.AspNetCore.Mvc;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyLib.Models.Users.Table;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.TokenService;
using SkymeyUserService.Services.User;

namespace SkymeyUserService.Interfaces.Users.Register
{
    public interface IUserServiceRegister
    {
        void UserServiceRegisterInit(ApplicationContext db, ITokenService tokenService);
        Task<UserResponse> IsValidUserInformation(RegisterModel registerModel);
        Task<IUserResponse> Register(RegisterModel registerModel);
    }
}
