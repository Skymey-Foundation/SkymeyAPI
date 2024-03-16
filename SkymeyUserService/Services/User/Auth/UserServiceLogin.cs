using Microsoft.EntityFrameworkCore;
using SkymeyLib.Enums;
using SkymeyLib.Enums.Users;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Table;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Auth;
using SkymeyUserService.Interfaces.Users.TokenService;

namespace SkymeyUserService.Services.User.Auth
{
    public class UserServiceLogin : IUserServiceLogin
    {
        private UserResponse _userResponse = new UserResponse();
        ApplicationContext _db;
        private USR_001? _USR_001;
        ITokenService _tokenService;

        public void UserServiceLoginInit(ApplicationContext db,
            ITokenService tokenService)
        {
            _tokenService = tokenService;
            _db = db;
        }

        public async Task<UserResponse> IsValidUserInformation(LoginModel model, ApplicationContext _db)
        {
            _USR_001 = await _db.USR_001.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefaultAsync();
            if (_USR_001 != null)
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

        public async Task<UserResponse> Login(LoginModel loginModel)
        {
            _userResponse = await IsValidUserInformation(loginModel, _db);
            if (_userResponse.ResponseType)
            {
                var refreshToken = _tokenService.GenerateRefreshToken();
                _USR_001.RefreshToken = refreshToken;
                _USR_001.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(1);
                await _db.SaveChangesAsync();
                _userResponse.AuthenticatedResponses = new AuthenticatedResponse { Token = _tokenService.GenerateJwtToken(loginModel.Email), RefreshToken = refreshToken };
            }
            return _userResponse;
        }

    }
}
