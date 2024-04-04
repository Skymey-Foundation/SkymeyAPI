using Amazon.Runtime;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using Skymey.Data;
using Skymey.Pages;
using SkymeyLib.Handlers.HTTPHandler;
using SkymeyLib.Interfaces.Users.Login;
using SkymeyLib.Middleware;
using SkymeyLib.Models.Users;
using SkymeyLib.Models.Users.Login;
using SkymeyLib;
using FluentValidation.AspNetCore;
using System.Reflection;
using Blazorise;
using Blazorise.FluentValidation;
using SkymeyLib.Models.Users.Register;
using BlazorSchool.Components.Web.Core;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Components.Authorization;

namespace Skymey
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterModelValidation>());
            builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginModelValidation>());
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();
            
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazorComponents();
            builder.Services.AddTransient<IValidateToken, ValidateTokenInfo>();
            builder.WebHost.UseUrls("http://localhost:5005;https://localhost:5006;");
            //builder.Services.AddHttpClient().AddHttpMessageHandler<HttpHandler>();
            //builder.Services.AddHttpClient<IValidateToken, ValidateTokenInfo>()
            //.AddHttpMessageHandler<HttpHandler>();
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerApi"));
            #region JWT
            builder.Configuration.AddJsonFile("appsettingsMain.json");
            builder.Configuration.AddJsonFile(builder.Configuration.GetSection("Config").Get<SkymeyLib.Models.Config>().Path);
            builder.Services.AddAuthorizationCore();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience"),
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:Key"))),
                        ValidateIssuerSigningKey = true
                    };
                });
            builder.Services.AddScoped<AuthenticationStateProvider,
    CustomAuthenticationStateProvider>();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ServerUrl"] ?? "https://localhost:5006") });
            #endregion

            //builder.Services.AddTransient<HttpHandler>();
            //builder.Services.AddTransient<FetchData>();

            //builder.Services.AddHttpClient("main", httpClient =>
            //{
            //    httpClient.BaseAddress = new Uri(builder.Configuration["ServerUrl"] ?? "");
            //})
            //.AddHttpMessageHandler<HttpHandler>();

            //builder.Services.AddScoped<IHttpClientServiceImplementation, HttpClientFactoryService>();
            builder.Services
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IHttpService, HttpService>()
                .AddScoped<ILocalStorageService, LocalStorageService>();
            builder.Services.AddTransient(x => {
                var apiUrl = new Uri("https://localhost:5003/api/SkymeyAPI/user");

                return new HttpClient() { BaseAddress = apiUrl };
            });
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseMiddleware<JWTMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToPage("/_Host");
            });
            await app.RunAsync();
        }
    }
}
