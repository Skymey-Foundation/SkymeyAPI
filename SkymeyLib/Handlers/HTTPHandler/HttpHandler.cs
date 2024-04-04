using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Handlers.HTTPHandler
{
    public class HttpHandler : DelegatingHandler
    {
        public HttpHandler() { }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            Console.WriteLine("HttpHandler");
            //System.Threading.Interlocked.Increment(ref _count);
            //request.Headers.Add("X-Custom-Header", _count.ToString());
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
