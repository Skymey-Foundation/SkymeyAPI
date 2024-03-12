using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using SkymeyLib.Models.Mongo.Config;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Auth;
using SkymeyUserService.Interfaces.Users.Register;
using SkymeyUserService.Interfaces.Users.TokenService;
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
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IUserServiceRegister _userServiceRegister;
        private MongoClient _mongoClient;
        private ApplicationContext _db;
        private readonly IOptions<MongoConfig> _options;
        private readonly ITokenService _tokenService;

        public UserController(IConfiguration configuration, 
            IUserService userService, 
            IUserServiceRegister userServiceRegister,
            IOptions<MongoConfig> options,
            ITokenService tokenService)
        {
            _options = options;
            _configuration = configuration;
            _userService = userService;
            _userServiceRegister = userServiceRegister;
            _mongoClient = new MongoClient(_options.Value.Server);
            _db = ApplicationContext.Create(_mongoClient.GetDatabase(_options.Value.Database));
            _tokenService = tokenService;
            _tokenService.configuration(_configuration);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            await using (ApplicationContext db = _db)
            {
                bool isValid = await _userServiceRegister.IsValidUserInformation(registerModel, _db);
                if (isValid)
                {
                    var refreshToken = _tokenService.GenerateRefreshToken();
                    await db.USR_001.AddAsync(new SkymeyLib.Models.Users.Table.USR_001
                    {
                        _id = ObjectId.GenerateNewId(),
                        Email = registerModel.Email,
                        Password = registerModel.Password,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
                    });
                    await db.SaveChangesAsync();
                    return Ok(new AuthenticatedResponse { Token = _tokenService.GenerateJwtToken(registerModel.Email), RefreshToken = _tokenService.GenerateRefreshToken() });
                }
                else {
                    return BadRequest("User already exist");
                }
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginData)
        {
            bool isValid = await _userService.IsValidUserInformation(loginData, _db);
            if (isValid)
            {
                var user = await _db.USR_001.Where(x=>x.Email==loginData.Email).FirstOrDefaultAsync();
                var refreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                await _db.SaveChangesAsync();
                return Ok(new AuthenticatedResponse{ Token = _tokenService.GenerateJwtToken(loginData.Email), RefreshToken = refreshToken });
            }
            return BadRequest("Please pass the valid Email and Password");
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(nameof(GetResult))]
        public IActionResult GetResult()
        {
            return Ok("API Validated");
        }
    }
}
