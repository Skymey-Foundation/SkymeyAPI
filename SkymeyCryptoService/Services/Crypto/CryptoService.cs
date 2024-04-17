using MongoDB.Driver;
using SkymeyCryptoService.Data;
using SkymeyCryptoService.Models.Crypto;
using SkymeyJobsLibs.Models.ActualPrices;
using SkymeyLib.Models;
using SkymeyLib.Models.Crypto.Tickers;

namespace SkymeyCryptoService.Services.Crypto
{
    public interface ICryptoService
    {
        private static MongoClient _mongoClient;
        private static ApplicationContext _db;
        public HashSet<CryptoActualPricesView> GetActualPrices();
        public HashSet<CryptoTickersView> GetTickers();
    }
    public class CryptoService : ICryptoService
    {
        private static MongoClient _mongoClient = new MongoClient(Config.MongoClientConnection);
        private static ApplicationContext _db = ApplicationContext.Create(_mongoClient.GetDatabase(Config.MongoDbDatabase));

        public HashSet<CryptoActualPricesView> GetActualPrices()
        {
            var resp_mongo = (from i in _db.CurrentPrices select i);
            HashSet<CryptoActualPricesView> resp = new HashSet<CryptoActualPricesView>();
            foreach (var item in resp_mongo)
            {
                resp.Add(new CryptoActualPricesView { Price = item.Price, Ticker = item.Ticker,Update = item.Update });
            }
            return resp;
        }
        public HashSet<CryptoTickersView> GetTickers()
        {
            var resp_mongo = (from i in _db.CryptoTickers select i);
            HashSet<CryptoTickersView> resp = new HashSet<CryptoTickersView>();
            foreach (var item in resp_mongo)
            {
                resp.Add(new CryptoTickersView { Ticker = item.Ticker, BaseAsset = item.BaseAsset, BaseAssetPrecision = item.BaseAssetPrecision, QuoteAsset = item.QuoteAsset, QuoteAssetPrecision = item.QuoteAssetPrecision, Update = item.Update });
            }
            return resp;
        }
    }
}
