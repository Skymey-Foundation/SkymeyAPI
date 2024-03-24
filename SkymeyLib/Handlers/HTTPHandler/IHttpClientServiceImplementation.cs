using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Handlers.HTTPHandler
{
    public interface IHttpClientServiceImplementation
    {
        Task Execute();
    }
}
