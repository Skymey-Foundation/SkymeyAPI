
using Microsoft.Extensions.Configuration;
using SkymeyLib.Models;
using SkymeyLib.Models.Mongo.Config;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Auth;
using SkymeyUserService.Interfaces.Users.Register;
using SkymeyUserService.Interfaces.Users.TokenService;
using SkymeyUserService.Middleware;
using SkymeyUserService.Services.User.Auth;
using SkymeyUserService.Services.User.TokenService;

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
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<IUserServiceLogin, UserServiceLogin>();
            builder.Services.AddTransient<IUserServiceRegister, UserServiceRegister>();
            builder.Services.AddTransient<ITokenService, TokenService>();
            var section = builder.Configuration.GetSection("MongoConfig");
            builder.Services.Configure<MongoConfig>(section);

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
