using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using SkymeyLib.Enums;
using SkymeyLib.Enums.Users;
using SkymeyLib.Interfaces.IServers;
using SkymeyLib.Interfaces.IUsers;
using SkymeyLib.Models.HTTPRequests;
using SkymeyLib.Models.Users.Login;
using SkymeyUserService.Interfaces.Users.Login;
using SkymeyUserService.Interfaces.Users.Register;
using SkymeyUserService.Services.User;
using System.Text.Json;

namespace SkymeyAPI.Controllers
{
    [Route("api/[controller]/User")]
    [ApiController]
    public class SkymeyAPIController : Controller
    {
        private readonly IUsers _users;
        public SkymeyAPIController(IUsers users) {
            _users = users;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<UserResponse> Login(LoginModel loginModel)
        {
            using (RestHTTP<UserResponse> respHTTP = new RestHTTP<UserResponse>(_users.Server, _users.Port, UserLogin.LoginUrl.StringValue(), Method.Post, loginModel))
            {
                return await respHTTP.Send();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("Get")]
        public string Get()
        {
            return "d";
        }
    }
}
