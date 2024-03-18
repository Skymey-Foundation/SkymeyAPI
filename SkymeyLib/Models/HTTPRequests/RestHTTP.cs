using Amazon.Runtime.Internal;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SkymeyLib.Models.HTTPRequests
{
    public class RestHTTP<T> : IDisposable
    {
        private RestClientOptions _options { get; set; }
        private RestClient _client;
        private RestRequest _request;
        public RestHTTP(string server, string port, string distance, Method method, object? json)
        {
            _options = new RestClientOptions(server+":"+port)
            {
                MaxTimeout = -1,
            };
            _client = new RestClient(_options);
            _request = new RestRequest(distance, method);
            if (json != null )
            {
                _request.AddHeader("Content-Type", "application/json");
                _request.AddStringBody(JsonSerializer.Serialize(json), DataFormat.Json);
            }
        }

        public async Task<T> Send()
        {
            var query = await _client.ExecuteAsync(_request);
            return JsonSerializer.Deserialize<T>(query.Content);
        }

        public void Dispose()
        {
            
        }
    }
}
