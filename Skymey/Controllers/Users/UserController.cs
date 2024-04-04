using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using SkymeyLib.Enums.Users;
using SkymeyLib.Models.HTTPRequests;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users;
using static System.Net.WebRequestMethods;
using System;
using System.Text.Json;
using Skymey.Data;
using Microsoft.AspNetCore.Authorization;

namespace Skymey.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        [HttpPost]
        [Route("Login")]
        public async Task<ObjectResult> Login(LoginModel loginModel)
        {
            HttpClient client = new HttpClient();
            using (var response = await client.PostAsJsonAsync("https://localhost:5003/api/SkymeyAPI/User/Login", loginModel))
            {
                using (var resp = JsonSerializer.Deserialize<UserResponse>(await response.Content.ReadAsStreamAsync()))
                {
                    return Ok(resp);
                }
            }
        }
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<ObjectResult> RefreshToken(ValidateToken valid_token)
        {
            HttpClient client = new HttpClient();
            using (var response = await client.PostAsJsonAsync("https://localhost:5003/api/SkymeyAPI/User/RefreshToken", valid_token))
            {
                using (var resp = JsonSerializer.Deserialize<UserResponse>(await response.Content.ReadAsStreamAsync()))
                {
                    return Ok(resp);
                }
            }
        }
        [HttpPost]
        [Route("Get")]
        [Authorize]
        public async Task<WeatherForecast[]> GetForecastAsync(DateOnly startDate)
        {
             string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
            return await Task.FromResult(Enumerable.Range(1, 50).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray());
        }
    }
}
