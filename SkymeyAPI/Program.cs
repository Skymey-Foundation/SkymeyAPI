
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SkymeyLib.Interfaces.HTTPRequests;
using SkymeyLib.Interfaces.ICrypto;
using SkymeyLib.Interfaces.IServers;
using SkymeyLib.Interfaces.IUsers;
using SkymeyLib.Middleware;
using SkymeyLib.Models;
using SkymeyLib.Models.HTTPRequests;
using SkymeyLib.Models.ServersSettings;
using SkymeyUserService.Interfaces.Users.Login;
using SkymeyUserService.Services.User.Login;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace SkymeyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls("http://localhost:5002;https://localhost:5003;");
            builder.Services.AddControllers();
            builder.Configuration.AddJsonFile("appsettingsAPI.json");
            builder.Configuration.AddJsonFile(builder.Configuration.GetSection("Config").Get<Config>().Path);
            
            builder.Services.AddSingleton<IUsers, Users>();
            builder.Services.AddSingleton<ICrypto, Crypto>();
            builder.Services.AddSingleton<IServers, Servers>();
            
            #region JWT
            builder.Services.AddAuthorization();
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
            #endregion
            #region Swagger Configuration
            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Skymey API",
                    Description = ".NET 8.0 Web API"
                });
                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text input below:",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });
            #endregion

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseMiddleware<JWTMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
