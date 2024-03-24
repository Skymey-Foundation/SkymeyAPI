using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyUserService.Interfaces.Users.Login;
using SkymeyUserService.Interfaces.Users.Register;
using System.Security.Claims;

namespace SkymeyUserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkymeyUserServiceController : Controller
    {
        private readonly IUserServiceLogin _userServiceLogin;
        private readonly IUserServiceRegister _userServiceRegister;

        public SkymeyUserServiceController(IUserServiceLogin userService, IUserServiceRegister userServiceRegister)
        {
            _userServiceLogin = userService;
            _userServiceRegister = userServiceRegister;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
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
        [HttpPost]
        [Route("Login")]
        public async Task<ObjectResult> Login(LoginModel loginData)
        {
            using (IUserResponse resp = await _userServiceLogin.Login(loginData))
            {
                if (resp.ResponseType)
                {
                    return StatusCode(Convert.ToInt32(resp.StatusCode), resp);
                }
                else
                {
                    return StatusCode(Convert.ToInt32(resp.StatusCode), resp);
                }
            }
        }

        [HttpGet,AllowAnonymous]
        [Route("GetResult")]
        public IActionResult GetResult()
        {
            return Ok(new LoginModel() { Email="email",Password="pwd"});
        }

        //[Authorize]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("ValidateToken")]
        public async Task<ObjectResult> ValidateToken(ValidateToken token)
        {
            using (IUserResponse resp = await _userServiceLogin.RefreshToken(token)) {
                return StatusCode(Convert.ToInt32(resp.StatusCode), resp);
            }
        }
    }
}
