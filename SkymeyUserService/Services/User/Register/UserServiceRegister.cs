using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkymeyLib.Enums;
using SkymeyLib.Enums.Users.Register;
using SkymeyLib.Models.Users;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyLib.Models.Users.Table;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Register;
using SkymeyUserService.Interfaces.Users.TokenService;
using System.Net;

namespace SkymeyUserService.Services.User.Register
{
    public class UserServiceRegister : IUserServiceRegister, IDisposable
    {
        private UserResponse _userResponse = new UserResponse() { StatusCode = HttpStatusCode.OK, AuthenticatedResponses = new AuthenticatedResponse() { } };
        ITokenService _tokenService;
        public UserServiceRegister(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<UserResponse> IsValidUserInformation(RegisterModel registerModel)
        {
            await using (ApplicationContext _db = new ApplicationContext())
            {
                if (await _db.USR_001.Where(x => x.Email == registerModel.Email).FirstOrDefaultAsync() == null)
                {
                    _userResponse.ResponseType = true;
                    _userResponse.Response = UserRegister.Ok.StringValue();
                    _userResponse.StatusCode = HttpStatusCode.OK;
                    return _userResponse;
                }
                else
                {
                    _userResponse.ResponseType = false;
                    _userResponse.Response = UserRegister.AlreadyExist.StringValue();
                    _userResponse.StatusCode = HttpStatusCode.BadRequest;
                    return _userResponse;
                }
            }
        }

        public async Task<IUserResponse> Register(RegisterModel registerModel)
        {
            _userResponse = await IsValidUserInformation(registerModel);
            if (_userResponse.ResponseType)
            {
                await using (ApplicationContext _db = new ApplicationContext())
                {

                    var refreshToken = _tokenService.GenerateRefreshToken();
                    await _db.USR_001.AddAsync(new USR_001
                    {
                        Email = registerModel.Email,
                        Password = registerModel.Password,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
                    });
                    await _db.SaveChangesAsync();
                    _userResponse.AuthenticatedResponses = new AuthenticatedResponse { Token = _tokenService.GenerateJwtToken(registerModel.Email), RefreshToken = refreshToken };
                }
            }
            return _userResponse;
        }

        #region Dispose, Ctor
        public void Dispose()
        {
            _userResponse.Dispose();
        }
        ~UserServiceRegister()
        {

        }
        #endregion
    }
}
