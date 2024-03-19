using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using SkymeyLib.Enums;
using SkymeyLib.Enums.Users;
using SkymeyLib.Enums.Users.Register;
using SkymeyLib.Interfaces.IServers;
using SkymeyLib.Interfaces.IUsers;
using SkymeyLib.Models.HTTPRequests;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyUserService.Interfaces.Users.Login;
using SkymeyUserService.Interfaces.Users.Register;
using SkymeyUserService.Services.User;
using System.Net;
using System.Text.Json;

namespace SkymeyAPI.Controllers
{
    [Route("api/[controller]/User")]
    [ApiController]
    public class SkymeyAPIController : Controller
    {
        private readonly IServers _servers;
        public SkymeyAPIController(IServers servers) {
            _servers = servers;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ObjectResult> Login(LoginModel loginModel)
        {
            using (RestHTTP<UserResponse> respHTTP = new RestHTTP<UserResponse>(_servers.UsersSettings.Server, _servers.UsersSettings.Port, UserLogin.LoginUrl.StringValue(), Method.Post, loginModel))
            {
                var resp = await respHTTP.Send();
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

        [HttpPost]
        [Route("Register")]
        public async Task<ObjectResult> Login(RegisterModel registerModel)
        {
            using (RestHTTP<UserResponse> respHTTP = new RestHTTP<UserResponse>(_servers.UsersSettings.Server, _servers.UsersSettings.Port, UserRegister.RegisterURL.StringValue(), Method.Post, registerModel))
            {
                var resp = await respHTTP.Send();
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

        [HttpGet]
        [Authorize]
        [Route("Get")]
        public string Get()
        {
            return "d";
        }
    }
}
