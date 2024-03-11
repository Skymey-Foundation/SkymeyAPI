using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Auth;
using SkymeyUserService.Interfaces.Users.Register;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public UserController(IConfiguration configuration, IUserService userService, IUserServiceRegister userServiceRegister)
        {
            _configuration = configuration;
            _userService = userService;
            _userServiceRegister = userServiceRegister;
            _mongoClient = new MongoClient("mongodb://127.0.0.1:27017");
            _db = ApplicationContext.Create(_mongoClient.GetDatabase("skymey"));
        }

        [HttpPost("Register")]
        public async Task<string> Register(RegisterModel registerModel)
        {
            await using (ApplicationContext db = _db)
            {
                bool isValid = await _userServiceRegister.IsValidUserInformation(registerModel, _db);
                if (isValid)
                {
                    await db.USR_001.AddAsync(new SkymeyLib.Models.Users.Table.USR_001
                    {
                        _id = ObjectId.GenerateNewId(),
                        Email = registerModel.Email,
                        Password = registerModel.Password
                    });
                    await db.SaveChangesAsync();
                    return "ok";
                }
                else {
                    return "not ok";
                }
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(LoginModel loginData)
        {
            bool isValid = _userService.IsValidUserInformation(loginData);
            if (isValid)
            {
                var tokenString = GenerateJwtToken(loginData.Email);
                return Ok(new { Token = tokenString, Message = "Success" });
            }
            return BadRequest("Please pass the valid Email and Password");
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(nameof(GetResult))]
        public IActionResult GetResult()
        {
            return Ok("API Validated");
        }

        private string GenerateJwtToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userName) }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
