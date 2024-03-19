using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Interfaces.HTTPRequests
{
    public interface IRestHTTP<T>
    {
        public Task<T> Send();
    }
}
