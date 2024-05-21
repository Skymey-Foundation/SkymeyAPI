using Microsoft.EntityFrameworkCore;
using SkymeyLib.Enums;
using SkymeyLib.Enums.Users;
using SkymeyLib.Models.Users;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Table;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Login;
using SkymeyUserService.Interfaces.Users.TokenService;
using System.Net;
using System.Security.Claims;

namespace SkymeyUserService.Services.User.Login
{
    public class UserServiceLogin : IUserServiceLogin, IDisposable
    {
        private UserResponse _userResponse = new UserResponse() { StatusCode = HttpStatusCode.OK, AuthenticatedResponses = new AuthenticatedResponse() { } };
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
                    _userResponse.StatusCode = HttpStatusCode.OK;
                    return _userResponse;
                }
                else
                {
                    _userResponse.ResponseType = false;
                    _userResponse.Response = UserLogin.LoginAndPassword.StringValue();
                    _userResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _userResponse;
                }
            }
        }

        public async Task<UserResponse> Login(LoginModel loginModel)
        {
            _userResponse = await IsValidUserInformation(loginModel);
            if (_userResponse.ResponseType)
            {
                await UpdateTokenForUser();
            }
            return _userResponse;
        }
        
        private async Task UpdateTokenForUser()
        {
            await using (ApplicationContext _db = new ApplicationContext())
            {
                if (_USR_001 != null)
                {
                    string? refreshToken = _tokenService.GenerateRefreshToken();
                    _USR_001.RefreshToken = refreshToken;
                    _USR_001.RefreshTokenExpiryTime = DateTime.Now.AddDays(60);
                    _db.USR_001.Update(_USR_001);
                    var role = (from i in _db.USR_GRP_003 select i).FirstOrDefault();
                    await _db.SaveChangesAsync();
                    _userResponse.AuthenticatedResponses = new AuthenticatedResponse { Token = _tokenService.GenerateJwtToken(_USR_001.Email, role.GRP_002_ID), RefreshToken = refreshToken };
                }
            }
        }

        public async Task<UserResponse> RefreshToken(ValidateToken token)
        {
            _tokenService.GetPrincipalFromExpiredToken(token.Token);
            if (await IsRefreshTokenOk(token.RefreshToken))
            {
                _userResponse.StatusCode = HttpStatusCode.OK;
                _userResponse.Response = "Ok";
                _userResponse.ResponseType = true;
                await UpdateTokenForUser();
                return _userResponse;
            }
            else
            {
                _userResponse.StatusCode = HttpStatusCode.BadRequest;
                _userResponse.Response = "Invalid token";
                _userResponse.ResponseType = false;
                return _userResponse;
            }
        }

        public async Task<bool> IsRefreshTokenOk(string refreshToken)
        {
            await using (ApplicationContext _db = new ApplicationContext())
            {
                _USR_001 = (from i in _db.USR_001 where i.RefreshToken == refreshToken select i).FirstOrDefault();
                if (_USR_001 is not null) { 
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #region Dispose, Ctor
        public void Dispose()
        {
        }
        ~UserServiceLogin()
        {

        }
        #endregion
    }
}
