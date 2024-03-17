using Microsoft.EntityFrameworkCore;
using SkymeyLib.Enums;
using SkymeyLib.Enums.Users;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Table;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Login;
using SkymeyUserService.Interfaces.Users.TokenService;

namespace SkymeyUserService.Services.User.Login
{
    public class UserServiceLogin : IUserServiceLogin, IDisposable
    {
        private UserResponse _userResponse = new UserResponse() { AuthenticatedResponses = new AuthenticatedResponse() { } };
        private USR_001? _USR_001;
        ITokenService _tokenService;

        public UserServiceLogin(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<UserResponse> IsValidUserInformation(LoginModel model)
        {
            await using (ApplicationContext _db = new ApplicationContext())
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
        }

        public async Task<UserResponse> Login(LoginModel loginModel)
        {
            _userResponse = await IsValidUserInformation(loginModel);
            if (_userResponse.ResponseType)
            {
                await using (ApplicationContext _db = new ApplicationContext())
                {
                    if (_USR_001 != null)
                    {
                        var refreshToken = _tokenService.GenerateRefreshToken();
                        _USR_001.RefreshToken = refreshToken;
                        _USR_001.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(20);
                        await _db.SaveChangesAsync();
                        _userResponse.AuthenticatedResponses = new AuthenticatedResponse { Token = _tokenService.GenerateJwtToken(loginModel.Email), RefreshToken = refreshToken };
                    }
                }
            }
            return _userResponse;
        }

        #region Dispose, Ctor
        public void Dispose()
        {
            _USR_001 = null;
            _userResponse.Dispose();
        }
        ~UserServiceLogin()
        {

        }
        #endregion
    }
}
