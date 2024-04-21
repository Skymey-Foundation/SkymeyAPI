using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkymeyCryptoService.Models.Crypto;
using SkymeyLib.Models.Crypto.CryptoInstruments;
using SkymeyLib.Models.Crypto.Tickers;
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

        [AllowAnonymous]
        [HttpGet("GetTickers")]
        public async Task<HashSet<CryptoTickersView>> GetTickers()
        {
            using (HttpClient http = new HttpClient())
            {
                return await http.GetFromJsonAsync<HashSet<CryptoTickersView>>("https://localhost:5016/api/Cryptoservice/GetTickers");
            }
        }

        [AllowAnonymous]
        [HttpGet("GetInstruments")]
        public async Task<HashSet<CryptoInstrumentsDB>> GetInstruments()
        {
            using (HttpClient http = new HttpClient())
            {
                return await http.GetFromJsonAsync<HashSet<CryptoInstrumentsDB>>("https://localhost:5016/api/Cryptoservice/GetInstruments");
            }
        }
    }
}
