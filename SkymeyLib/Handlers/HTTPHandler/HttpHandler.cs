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
            //System.Threading.Interlocked.Increment(ref _count);
            //request.Headers.Add("X-Custom-Header", _count.ToString());
            Console.WriteLine(request);
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler");
            Console.WriteLine("handler2");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
