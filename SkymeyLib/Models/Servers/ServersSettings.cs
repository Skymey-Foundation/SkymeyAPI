using Microsoft.Extensions.Configuration;
using SkymeyLib.Interfaces.ICrypto;
using SkymeyLib.Interfaces.IServers;
using SkymeyLib.Interfaces.IUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Models.ServersSettings
{
    public class Servers : IServers
    {
        public Users? UsersSettings { get; set; }
        public Crypto? CryptoSettings { get; set; }

        public Servers(IUsers users, ICrypto crypto) {
            UsersSettings = (Users)users;
            CryptoSettings = (Crypto)crypto;
            Console.Write(CryptoSettings);
        }
    }

    public class Users : IUsers
    {
        public required string Server { get; set; }
        public required string Port { get; set; }
        private readonly IConfiguration _configuration;

        public Users(IConfiguration configuration)
        {
            _configuration = configuration;
            Server = _configuration.GetSection("Servers:Users:Server").Get<string>();
            Port = _configuration.GetSection("Servers:Users:Port").Get<string>();
        }
    }

    public class Crypto : ICrypto
    {
        public required string Server { get; set; }
        public required string Port { get; set; }
        private readonly IConfiguration _configuration;
        public Crypto(IConfiguration configuration)
        {
            _configuration = configuration;
            Server = _configuration.GetSection("Servers:Crypto:Server").Get<string>();
            Port = _configuration.GetSection("Servers:Crypto:Port").Get<string>();
        }
    }
}
