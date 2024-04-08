using Microsoft.AspNetCore.Mvc;
using SkymeyCryptoService.Models.Crypto;
using SkymeyCryptoService.Services.Crypto;

namespace SkymeyCryptoService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly ILogger<CryptoController> _logger;
        private readonly ICryptoService _cryptoService;

        public CryptoController(ILogger<CryptoController> logger, ICryptoService cryptoService)
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
    }
}
