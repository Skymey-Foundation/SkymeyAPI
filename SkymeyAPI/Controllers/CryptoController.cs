using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkymeyCryptoService.Models.Crypto;
using System.Collections.Generic;

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
        [HttpGet("GetActualPrices")]
        public async Task<HashSet<CryptoActualPricesView>> GetActualPrices()
        {
            using (HttpClient http = new HttpClient())
            {
                return await http.GetFromJsonAsync<HashSet<CryptoActualPricesView>>("https://localhost:5016/api/Cryptoservice/GetActualPrices");
            }
        }
    }
}
