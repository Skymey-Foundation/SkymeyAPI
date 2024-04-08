using MongoDB.Driver;
using SkymeyCryptoService.Data;
using SkymeyCryptoService.Models.Crypto;
using SkymeyJobsLibs.Models.ActualPrices;
using SkymeyLib.Models;

namespace SkymeyCryptoService.Services.Crypto
{
    public interface ICryptoService
    {
        private static MongoClient _mongoClient;
        private static ApplicationContext _db;
        public HashSet<CryptoActualPricesView> GetActualPrices();
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
    }
}
