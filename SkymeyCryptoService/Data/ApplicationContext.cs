using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using SkymeyJobsLibs.Models.ActualPrices;
using SkymeyLib.Models;
using SkymeyLib.Models.Crypto.CryptoInstruments;
using SkymeyLib.Models.Crypto.Tickers;
using SkymeyLib.Models.Users.Table;

namespace SkymeyCryptoService.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<CurrentPrices> CurrentPrices { get; init; }
        public DbSet<CryptoTickers> CryptoTickers { get; init; }
        public DbSet<CryptoInstrumentsDB> CryptoInstrumentsDB { get; init; }
        public static ApplicationContext Create(IMongoDatabase database) =>
            new(new DbContextOptionsBuilder<ApplicationContext>()
                .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
                .Options);

        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CurrentPrices>().ToCollection("crypto_current_prices");
            modelBuilder.Entity<CryptoTickers>().ToCollection("crypto_tickers");
            modelBuilder.Entity<CryptoInstrumentsDB>().ToCollection("crypto_instruments");
        }
    }
}
