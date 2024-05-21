using Microsoft.AspNetCore.Mvc;
using SkymeyCryptoService.Models.Crypto;
using SkymeyCryptoService.Services.Crypto;
using SkymeyLib.Models.Crypto.Blockchains;
using SkymeyLib.Models.Crypto.CryptoInstruments;
using SkymeyLib.Models.Crypto.Tickers;
using SkymeyLib.Models.Crypto.Tokens;

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

        [HttpGet]
        [Route("GetInstruments")]
        public HashSet<CryptoInstrumentsDB> GetInstruments()
        {
            return _cryptoService.GetInstruments();
        }

        [HttpGet]
        [Route("GetInstruments/{ticker}")]
        public CryptoInstrumentsDB GetInstruments(string ticker)
        {
            return _cryptoService.GetInstruments(ticker);
        }

        [HttpPost]
        [Route("AddInstruments")]
        public CryptoInstrumentsDB AddInstruments(CryptoInstrumentsDB instrument)
        {
            return _cryptoService.AddInstruments(instrument);
        }

        [HttpPost]
        [Route("AddContract")]
        public Tokens AddContract(Tokens contract)
        {
            return _cryptoService.AddContract(contract);
        }

        [HttpPost]
        [Route("AddBlockchain")]
        public BLOCK_004 AddBlockchain(BLOCK_004 blockchain)
        {
            return _cryptoService.AddBlockchain(blockchain);
        }

        [HttpGet]
        [Route("GetBlockchains")]
        public async Task<List<BLOCK_004>> GetBlockchains()
        {
            return _cryptoService.GetBlockchains();
        }
    }
}
