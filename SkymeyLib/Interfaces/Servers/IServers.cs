using SkymeyLib.Models.ServersSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Interfaces.IServers
{
    public interface IServers
    {
        public Users? UsersSettings { get; set; }
        public Crypto? CryptoSettings { get; set; }
    }
}
