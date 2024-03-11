using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Models.Mongo.Config
{
    public interface IMongoConfig
    {
        public string Server {  get; set; }
        public string Database { get; set; }
    }
}
