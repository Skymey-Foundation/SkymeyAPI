using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkymeyLib.Interfaces.IServers;

namespace SkymeyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IServers _servers;
        public UserController(IServers servers) {
            _servers = servers;
        }
        [HttpGet("Login123")]
        public string Login()
        {
            return "ok";
        }

        [HttpGet("Get")]
        [Authorize]
        public string Get()
        {
            return "d";
        }
    }
}
