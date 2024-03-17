using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Interfaces.ICrypto
{
    public interface ICrypto
    {
        string? Server { get; set; }
        string? Port { get; set; }
    }
}
