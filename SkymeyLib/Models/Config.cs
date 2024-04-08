using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Models
{
    public class Config
    {
        public required string Path { get; set; }
        public static string MongoDbDatabase { get; set; }
        public static string MongoClientConnection { get; set; }
    }
}
