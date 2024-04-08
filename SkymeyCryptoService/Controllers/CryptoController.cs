using Microsoft.AspNetCore.Mvc;

namespace SkymeyCryptoService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly ILogger<CryptoController> _logger;

        public CryptoController(ILogger<CryptoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task GetActualPrices()
        {

        }
    }
}
