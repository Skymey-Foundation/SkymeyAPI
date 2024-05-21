using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkymeyCryptoService.Models.Crypto;
using SkymeyLib.Models.Crypto.CryptoInstruments;
using SkymeyLib.Models.Crypto.Tickers;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System;
using System.Net.Http;
using System.Text.Json;
using SkymeyLib.Models.Crypto.Tokens;
using SkymeyLib.Models.Crypto.Blockchains;

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

        [AllowAnonymous]
        [HttpGet("GetInstruments/{ticker}")]
        public async Task<CryptoInstrumentsDB> GetInstruments(string ticker)
        {
            using (HttpClient http = new HttpClient())
            {
                return await http.GetFromJsonAsync<CryptoInstrumentsDB>("https://localhost:5016/api/Cryptoservice/GetInstruments/" + ticker);
            }
        }

        [AllowAnonymous]
        [HttpPost("AddInstruments")]
        public async Task<object> AddInstrumentsAsync(CryptoInstrumentsDB instrument)
        {
            using (HttpClient http = new HttpClient())
            {
                JsonContent json = JsonContent.Create(instrument);
                using var response = await http.PostAsync("https://localhost:5016/api/Cryptoservice/AddInstruments", json);
                return response;
            }
        }

        [AllowAnonymous]
        [HttpPost("AddContract")]
        public async Task<object> AddContract(Tokens contract)
        {
            using (HttpClient http = new HttpClient())
            {
                JsonContent json = JsonContent.Create(contract);
                using var response = await http.PostAsync("https://localhost:5016/api/Cryptoservice/AddContract", json);
                return response;
            }
        }

        [AllowAnonymous]
        [HttpPost("AddBlockchain")]
        public async Task<object> AddBlockchain(BLOCK_004 blockchain)
        {
            using (HttpClient http = new HttpClient())
            {
                JsonContent json = JsonContent.Create(blockchain);
                using var response = await http.PostAsync("https://localhost:5016/api/Cryptoservice/AddBlockchain", json);
                return response;
            }
        }

        [AllowAnonymous]
        [HttpGet("GetBlockchains")]
        public async Task<List<BLOCK_004>> GetBlockchains()
        {
            using (HttpClient http = new HttpClient())
            {
                return await http.GetFromJsonAsync<List<BLOCK_004>>("https://localhost:5016/api/Cryptoservice/GetBlockchains/");
            }
        }

        [AllowAnonymous]
        [Route("/ws")]
        [HttpGet]
        public async Task GetWS()
        {
            Console.WriteLine("request endpoint");
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                Console.WriteLine("request ws");
                using var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
                while (true)
                {
                    Console.WriteLine("while ws");
                    var message = "The time is: " + DateTime.Now.ToString("HH:mm:ss");
                    var bytes = Encoding.UTF8.GetBytes(message);
                    var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                    if (ws.State == WebSocketState.Open)
                    {
                        await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                        Console.WriteLine(arraySegment);
                    }
                    else if (ws.State == WebSocketState.Closed ||
                             ws.State == WebSocketState.Aborted)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
