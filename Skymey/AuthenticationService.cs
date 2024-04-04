using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using SkymeyLib.Models.ServersSettings;
using SkymeyLib.Models.Users.Login;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Collections.Specialized;
using System.Web;
using SkymeyLib.Models.Users;

namespace Skymey
{
    public static class ExtensionMethods
    {
        public static NameValueCollection QueryString(this NavigationManager navigationManager)
        {
            return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
        }

        public static string QueryString(this NavigationManager navigationManager, string key)
        {
            return navigationManager.QueryString()[key];
        }
    }
    public class User
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
    public interface IAuthenticationService
    {
        User User { get; }
        Task Initialize();
        Task Login(string username, string password);
        Task Logout();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private IHttpService _httpService;
        private ILocalStorageService _localStorageService;

        public User User { get; private set; }

        public AuthenticationService(
            IHttpService httpService,
            ILocalStorageService localStorageService
        )
        {
            _httpService = httpService;
            _localStorageService = localStorageService;
        }

        public async Task Initialize()
        {
            User = await _localStorageService.GetItem<User>("user");
        }

        public async Task Login(string username, string password)
        {
            var _User = await _httpService.Post<UserResponse>("https://localhost:5003/api/SkymeyAPI/user/login", new LoginModel{ Email=username, Password= password });
            User = new User
            {
                Token = _User.AuthenticatedResponses.Token
            ,
                RefreshToken = _User.AuthenticatedResponses.RefreshToken
            ,
                Email = username
            };
            await _localStorageService.SetItem("user", User);
        }

        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItem("user");
        }
    }
    public interface IHttpService
    {
        Task<T> Get<T>(string uri);
        Task<T> Post<T>(string uri, object value);
    }

    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private IConfiguration _configuration;

        public HttpService(
            HttpClient httpClient,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService,
            IConfiguration configuration
        )
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _configuration = configuration;
        }

        public async Task<T> Get<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest<T>(request);
        }

        public async Task<T> Post<T>(string uri, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return await sendRequest<T>(request);
        }

        // helper methods

        private async Task<T> sendRequest<T>(HttpRequestMessage request)
        {
            // add jwt auth header if user is logged in and request is to the api url
            var user = await _localStorageService.GetItem<User>("user");
            var isApiUrl = !request.RequestUri.IsAbsoluteUri;
            if (user != null && isApiUrl)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            using var response = await _httpClient.SendAsync(request);

            // auto logout on 401 response
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _navigationManager.NavigateTo("logout");
                return default;
            }

            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
    public interface ILocalStorageService
    {
        Task<T> GetItem<T>(string key);
        Task SetItem<T>(string key, T value);
        Task RemoveItem(string key);
    }

    public class LocalStorageService : ILocalStorageService
    {
        private IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<T> GetItem<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (json == null)
                return default;

            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task SetItem<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }

        public async Task RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
    public interface IUserService
    {
        Task<IEnumerable<LoginModel>> GetAll();
    }

    public class UserService : IUserService
    {
        private IHttpService _httpService;

        public UserService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<LoginModel>> GetAll()
        {
            return await _httpService.Get<IEnumerable<LoginModel>>("/users");
        }
    }
}
