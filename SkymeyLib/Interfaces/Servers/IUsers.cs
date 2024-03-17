using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Interfaces.IUsers
{
    public interface IUsers
    {
        string? Server { get; set; }
        string? Port { get; set; }
    }
}
