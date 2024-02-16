using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SkymeyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoController : Controller
    {
        [HttpGet("Get_order")]
        public string Get()
        {
            return "ok";
        }
    }
}
