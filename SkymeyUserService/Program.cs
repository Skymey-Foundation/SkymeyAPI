
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using SkymeyLib.Models;
using SkymeyLib.Models.Mongo.Config;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Login;
using SkymeyUserService.Interfaces.Users.Register;
using SkymeyUserService.Interfaces.Users.TokenService;
using SkymeyUserService.Services.User.Login;
using SkymeyUserService.Services.User.Register;
using SkymeyUserService.Services.User.TokenService;
using System.Text;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.SwaggerGen;
using SkymeyLib.Middleware;

namespace SkymeyUserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls("http://localhost:5000;https://localhost:5001;");
            builder.Configuration.AddJsonFile(builder.Configuration.GetSection("Config").Get<Config>().Path);
            
            builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddSingleton<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserServiceLogin, UserServiceLogin>();
            builder.Services.AddScoped<IUserServiceRegister, UserServiceRegister>();

            #region JWT
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
            builder.Services.AddAuthorization();
            #endregion
            #region Swagger Configuration
            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SkymeyUserService",
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        internal class SwaggerDocumentFilter : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                foreach (var apiDescription in context.ApiDescriptions)
                {
                    var controllerActionDescriptor = (ControllerActionDescriptor)apiDescription.ActionDescriptor;

                    // If the namespace of the controller DOES NOT start with..
                    if (!controllerActionDescriptor.ControllerTypeInfo.FullName.StartsWith("SkymeyUserService"))
                    {
                        var key = "/" + apiDescription.RelativePath.TrimEnd('/');
                        swaggerDoc.Paths.Remove(key); // Hides the Api
                    }
                }
            }
        }
    }
}
