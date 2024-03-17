using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using SkymeyLib.Enums;
using SkymeyLib.Enums.Users;
using SkymeyLib.Models.Mongo.Config;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyLib.Models.Users.Table;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Login;
using SkymeyUserService.Interfaces.Users.Register;
using SkymeyUserService.Interfaces.Users.TokenService;
using SkymeyUserService.Services.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SkymeyUserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserServiceLogin _userServiceLogin;
        private readonly IUserServiceRegister _userServiceRegister;

        public UserController(IUserServiceLogin userService, IUserServiceRegister userServiceRegister)
        {
            _userServiceLogin = userService;
            _userServiceRegister = userServiceRegister;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            using (IUserResponse resp = await _userServiceRegister.Register(registerModel))
            {
                if (resp.ResponseType)
                {
                    return Ok(resp);
                }
                else
                {
                    return BadRequest(resp);
                }
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginData)
        {
            using (IUserResponse resp = await _userServiceLogin.Login(loginData))
            {
                if (resp.ResponseType)
                {
                    return Ok(resp);
                }
                else
                {
                    return BadRequest(resp);
                }
            }
        }

        [HttpGet,Authorize,Route("GetResult")]
        public IActionResult GetResult()
        {
            return Ok("API Validated");
        }

        //[Authorize]
        ////[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        //[HttpGet(nameof(ValidateToken))]
        //public Claim ValidateToken(string token)
        //{
        //    var resp = _tokenService.GetPrincipalFromExpiredToken(token).Claims.FirstOrDefault();
        //    return resp;
        //}
    }
}
