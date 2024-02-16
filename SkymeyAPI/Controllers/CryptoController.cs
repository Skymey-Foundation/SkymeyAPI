using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SkymeyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoController : Controller
    {
        [Authorize]
        [HttpGet("Get_order")]
        public string Get()
        {
            return "ok";
        }

        [AllowAnonymous]
        [HttpGet("Get_order2")]
        public string Get2()
        {
            return "ok";
        }
    }
}
