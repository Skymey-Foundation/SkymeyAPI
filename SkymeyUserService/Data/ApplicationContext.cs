using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using SkymeyLib.Models;
using SkymeyLib.Models.Users.Table;

namespace SkymeyUserService.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<USR_001> USR_001 { get; init; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<USR_001>().ToCollection("USR_001");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettingsUserService.json");
            var sectionpath = builder.Build().GetSection("Config");
            builder.AddJsonFile(sectionpath.Get<Config>().Path);
            optionsBuilder.UseMongoDB(builder.Build().GetSection("MongoConfig:Server").Value, builder.Build().GetSection("MongoConfig:Database").Value);
        }
    }
}
