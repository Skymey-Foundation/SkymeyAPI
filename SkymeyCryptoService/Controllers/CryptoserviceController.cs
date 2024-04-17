using Microsoft.AspNetCore.Mvc;
using SkymeyCryptoService.Models.Crypto;
using SkymeyCryptoService.Services.Crypto;
using SkymeyLib.Models.Crypto.Tickers;

namespace SkymeyCryptoService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoserviceController : ControllerBase
    {
        private readonly ILogger<CryptoserviceController> _logger;
        private readonly ICryptoService _cryptoService;

        public CryptoserviceController(ILogger<CryptoserviceController> logger, ICryptoService cryptoService)
        {
            _logger = logger;
            _cryptoService = cryptoService;
        }

        [HttpGet]
        [Route("GetActualPrices")]
        public HashSet<CryptoActualPricesView> GetActualPrices()
        {
            return _cryptoService.GetActualPrices();
        }

        [HttpGet]
        [Route("GetTickers")]
        public HashSet<CryptoTickersView> GetTickers()
        {
            return _cryptoService.GetTickers();
        }
    }
}
