
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SkymeyLib.Models;
using SkymeyLib.Models.Mongo.Config;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Auth;
using SkymeyUserService.Interfaces.Users.Register;
using SkymeyUserService.Interfaces.Users.TokenService;
using SkymeyUserService.Middleware;
using SkymeyUserService.Services.User.Auth;
using SkymeyUserService.Services.User.TokenService;
using System.Text;
using System.Text.Json.Serialization;

namespace SkymeyUserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            var sectionpath = builder.Configuration.GetSection("Config");
            builder.Configuration.AddJsonFile(sectionpath.Get<Config>().Path);
            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddTransient<IUserServiceLogin, UserServiceLogin>();
            builder.Services.AddTransient<IUserServiceRegister, UserServiceRegister>();
            builder.Services.AddTransient<ITokenService, TokenService>();
            var section = builder.Configuration.GetSection("MongoConfig");
            builder.Services.Configure<MongoConfig>(section);
            
            Console.WriteLine(builder.Configuration.GetValue<string>("JWT:Audience"));
            Console.WriteLine(builder.Configuration.GetValue<string>("JWT:Issuer"));
            Console.WriteLine(builder.Configuration.GetValue<string>("JWT:Key"));
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
            #region Swagger Configuration
            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "JWT Token Authentication API",
                    Description = "ASP.NET Core 5.0 Web API"
                });
                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<JWTMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
