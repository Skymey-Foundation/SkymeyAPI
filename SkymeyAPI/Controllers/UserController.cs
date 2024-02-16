using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SkymeyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        [HttpGet("Login")]
        public string Login()
        {
            return "ok";
        }
    }
}
