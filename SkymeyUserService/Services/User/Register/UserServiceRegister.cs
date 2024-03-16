using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkymeyLib.Enums;
using SkymeyLib.Enums.Users.Register;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyLib.Models.Users.Table;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Register;
using SkymeyUserService.Interfaces.Users.TokenService;

namespace SkymeyUserService.Services.User.Auth
{
    public class UserServiceRegister : IUserServiceRegister
    {
        private UserResponse _userResponse = new UserResponse();
        ApplicationContext _db;
        ITokenService _tokenService;
        public void UserServiceRegisterInit(ApplicationContext db,
            ITokenService tokenService)
        {
            _tokenService = tokenService;
            _db = db;
        }

        public async Task<UserResponse> IsValidUserInformation(RegisterModel registerModel)
        {
            if (await _db.USR_001.Where(x => x.Email == registerModel.Email).FirstOrDefaultAsync() == null)
            {
                _userResponse.ResponseType = true;
                _userResponse.Response = UserRegister.Ok.StringValue();
                return _userResponse;
            }
            else
            {
                _userResponse.ResponseType = false;
                _userResponse.Response = UserRegister.AlreadyExist.StringValue();
                return _userResponse;
            }
        }

        public async Task<IUserResponse> Register(RegisterModel registerModel)
        {
            _userResponse = await IsValidUserInformation(registerModel);
            if (_userResponse.ResponseType)
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
            return _userResponse;
        }
    }
}
