using Microsoft.Extensions.Configuration;
using SkymeyLib.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SkymeyCryptoService
{
    public class MainSettingsStocks : IMainSettingsStocks
    {
        private IConfiguration? _configure;
        public MainSettingsStocks(IConfiguration? configure)
        {
            _configure = configure;
        }

        #region RESERVED
        public void Init()
        {
            //var json = JsonSerializer.Deserialize<Binance>(File.ReadAllText(Config.Path));
            Config.MongoDbDatabase = _configure.GetSection("MongoDbDatabase").Value;
            Config.MongoClientConnection = _configure.GetSection("MongoClientConnection").Value;
            Console.WriteLine(Config.MongoDbDatabase);
        }
        #endregion
    }

    public interface IMainSettingsStocks
    {
        public void Init();
    }
}