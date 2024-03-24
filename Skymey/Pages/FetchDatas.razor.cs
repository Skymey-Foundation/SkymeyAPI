using Skymey.Data;
using SkymeyLib.Handlers.HTTPHandler;
using static System.Net.WebRequestMethods;

namespace Skymey.Pages
{
    public partial class FetchDatas : IDisposable
    {
        private WeatherForecast[]? forecasts;
        private HttpClient _httpClient;
        
        public void Dispose()
        {
            Console.WriteLine("Dispose");
        }

    }
}
