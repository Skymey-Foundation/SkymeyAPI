using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using SkymeyCryptoService.Data;
using SkymeyCryptoService.Models.Crypto;
using SkymeyJobsLibs.Models.ActualPrices;
using SkymeyLib.Models;
using SkymeyLib.Models.Crypto.Blockchains;
using SkymeyLib.Models.Crypto.CryptoInstruments;
using SkymeyLib.Models.Crypto.Tickers;
using SkymeyLib.Models.Crypto.Tokens;

namespace SkymeyCryptoService.Services.Crypto
{
    public interface ICryptoService
    {
        public HashSet<CryptoActualPricesView> GetActualPrices();
        public HashSet<CryptoTickersView> GetTickers();
        public HashSet<CryptoInstrumentsDB> GetInstruments();
        public List<BLOCK_004> GetBlockchains();
        public CryptoInstrumentsDB GetInstruments(string ticker);
        public CryptoInstrumentsDB AddInstruments(CryptoInstrumentsDB instrument);
        public BLOCK_004 AddBlockchain(BLOCK_004 blockchain);
        public Tokens AddContract(Tokens contract);
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
        public CryptoInstrumentsDB GetInstruments(string ticker)
        {
            return (from i in _db.CryptoInstrumentsDB where i.Symbol == ticker select i).AsNoTracking().FirstOrDefault();
        }
        public List<BLOCK_004> GetBlockchains()
        {
            return (from i in _db.BLOCK_004 select i).AsNoTracking().ToList();
        }
        public CryptoInstrumentsDB AddInstruments(CryptoInstrumentsDB instrument)
        {
            instrument._id = ObjectId.GenerateNewId();
            _db.CryptoInstrumentsDB.Add(instrument);
            _db.Tokens.Add(new Tokens { _id = ObjectId.GenerateNewId(),  InstrumentId = instrument.Id, Blockchain = instrument.Platform.Name, Contract = instrument.Platform.Token_address, Update = DateTime.UtcNow });
            _db.SaveChanges();
            return instrument;
        }
        public Tokens AddContract(Tokens contract)
        {
            contract._id = ObjectId.GenerateNewId();
            contract.Update = DateTime.UtcNow;
            _db.Tokens.Add(contract);
            _db.SaveChanges();
            return contract;
        }
        public BLOCK_004 AddBlockchain(BLOCK_004 blockchain)
        {
            blockchain.Update = DateTime.UtcNow;
            _db.BLOCK_004.Add(blockchain);
            _db.SaveChanges();
            return blockchain;
        }
    }
}
