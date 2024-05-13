using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SkymeyCryptoService.Data;
using SkymeyCryptoService.Models.Crypto;
using SkymeyJobsLibs.Models.ActualPrices;
using SkymeyLib.Models;
using SkymeyLib.Models.Crypto.CryptoInstruments;
using SkymeyLib.Models.Crypto.Tickers;

namespace SkymeyCryptoService.Services.Crypto
{
    public interface ICryptoService
    {
        public HashSet<CryptoActualPricesView> GetActualPrices();
        public HashSet<CryptoTickersView> GetTickers();
        public HashSet<CryptoInstrumentsDB> GetInstruments();
    }
    public class CryptoService : ICryptoService
    {
        static MongoClient _mongoClient = new MongoClient(Config.MongoClientConnection);
        ApplicationContext _db = ApplicationContext.Create(_mongoClient.GetDatabase(Config.MongoDbDatabase));

        public HashSet<CryptoActualPricesView> GetActualPrices()
        {
            var resp_mongo = (from i in _db.CurrentPrices select i).AsNoTracking();
            HashSet<CryptoActualPricesView> resp = new HashSet<CryptoActualPricesView>();
            foreach (var item in resp_mongo)
            {
                resp.Add(new CryptoActualPricesView { Price = item.Price, Ticker = item.Ticker,Update = item.Update });
            }
            return resp;
        }
        public HashSet<CryptoTickersView> GetTickers()
        {
            var resp_mongo = (from i in _db.CryptoTickers select i).AsNoTracking();
            HashSet<CryptoTickersView> resp = new HashSet<CryptoTickersView>();
            foreach (var item in resp_mongo)
            {
                resp.Add(new CryptoTickersView { Ticker = item.Ticker, BaseAsset = item.BaseAsset, BaseAssetPrecision = item.BaseAssetPrecision, QuoteAsset = item.QuoteAsset, QuoteAssetPrecision = item.QuoteAssetPrecision, Update = item.Update });
            }
            return resp;
        }
        public HashSet<CryptoInstrumentsDB> GetInstruments()
        {
            var resp_mongo = (from i in _db.CryptoInstrumentsDB select i).AsNoTracking();
            HashSet<CryptoInstrumentsDB> resp = new HashSet<CryptoInstrumentsDB>();
            foreach (var item in resp_mongo)
            {
                resp.Add(new CryptoInstrumentsDB { Id = item.Id, Is_active = item.Is_active, Name = item.Name, Rank = item.Rank, Slug = item.Slug, Symbol = item.Symbol, Update = item.Update, Platform = item.Platform });
            }
            return resp;
        }
    }
}
