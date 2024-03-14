using Microsoft.EntityFrameworkCore;
using SkymeyLib.Enums;
using SkymeyLib.Enums.Users;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Auth;

namespace SkymeyUserService.Services.User.Auth
{
    public class UserService : IUserService
    {
        private readonly UserResponse _userResponse = new UserResponse();
        public string GetUserDetails()
        {
            return "ok";
        }

        public async Task<UserResponse> IsValidUserInformation(LoginModel model, ApplicationContext _db)
        {
            if (await _db.USR_001.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefaultAsync() != null)
            {
                _userResponse.ResponseType = true;
                _userResponse.Response = UserLogin.Ok.StringValue();
                return _userResponse;
            }
            else
            {
                _userResponse.ResponseType = false;
                _userResponse.Response = UserLogin.LoginAndPassword.StringValue();
                return _userResponse;
            }
        }
    }
}
