using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Skymey
{
    public class AppRouteView : RouteView
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            var authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;
            if (authorize && AuthenticationService.User == null)
            {
                var returnUrl = WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery);
                NavigationManager.NavigateTo($"user/login?returnUrl={returnUrl}");
            }
            else
            {
                base.Render(builder);
            }
        }
    }
    public class FakeBackendHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var users = new[] { new { Id = 1, Email = "dsotnikov1996@gmail.com", Password = "Dsotnikov199^"} };
            var path = request.RequestUri.AbsolutePath;
            var method = request.Method;

            if (path == "/users/login" && method == HttpMethod.Post)
            {
                return await authenticate();
            }
            else if (path == "/users" && method == HttpMethod.Get)
            {
                return await getUsers();
            }
            else
            {
                // pass through any requests not handled above
                return await base.SendAsync(request, cancellationToken);
            }

            // route functions

            async Task<HttpResponseMessage> authenticate()
            {
                var bodyJson = await request.Content.ReadAsStringAsync();
                var body = JsonSerializer.Deserialize<Dictionary<string, string>>(bodyJson);
                var user = users.FirstOrDefault(x => x.Email == body["email"] && x.Password == body["password"]);

                if (user == null)
                    return await error("Username or password is incorrect");

                return await ok(new
                {
                    Id = user.Id,
                    Email = user.Email,
                    Token = "fake-jwt-token"
                });
            }

            async Task<HttpResponseMessage> getUsers()
            {
                if (!isLoggedIn()) return await unauthorized();
                return await ok(users);
            }

            // helper functions

            async Task<HttpResponseMessage> ok(object body)
            {
                return await jsonResponse(HttpStatusCode.OK, body);
            }

            async Task<HttpResponseMessage> error(string message)
            {
                return await jsonResponse(HttpStatusCode.BadRequest, new { message });
            }

            async Task<HttpResponseMessage> unauthorized()
            {
                return await jsonResponse(HttpStatusCode.Unauthorized, new { message = "Unauthorized" });
            }

            async Task<HttpResponseMessage> jsonResponse(HttpStatusCode statusCode, object content)
            {
                var response = new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json")
                };

                // delay to simulate real api call
                await Task.Delay(500);

                return response;
            }

            bool isLoggedIn()
            {
                return request.Headers.Authorization?.Parameter == "fake-jwt-token";
            }
        }
    }
}
