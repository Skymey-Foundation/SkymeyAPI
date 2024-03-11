
using Microsoft.Extensions.Configuration;
using SkymeyLib.Models.Mongo.Config;
using SkymeyUserService.Interfaces.Users.Auth;
using SkymeyUserService.Interfaces.Users.Register;
using SkymeyUserService.Middleware;
using SkymeyUserService.Services.User.Auth;

namespace SkymeyUserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            var sectionpath = builder.Configuration.GetSection("Config");
            var weatherClientConfig = sectionpath.Get<SkymeyLib.Models.Config>();
            builder.Configuration.AddJsonFile(weatherClientConfig.Path);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IUserServiceRegister, UserServiceRegister>();
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
