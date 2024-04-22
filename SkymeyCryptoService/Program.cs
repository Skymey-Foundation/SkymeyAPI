using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using SkymeyLib.Models;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using SkymeyCryptoService.Services.Crypto;
using SkymeyCryptoService;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5015;https://localhost:5016;");
try
{
    builder.Configuration.AddJsonFile("appsettingsCryptoService.json");
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
finally { }
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Add services to the container.

builder.Services.AddControllers();
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
});

#endregion
builder.Services.AddSingleton<IMainSettingsStocks, MainSettingsStocks>();
builder.Services.AddTransient<ICryptoService, CryptoService>();
using var serviceProvider = builder.Services.BuildServiceProvider();
var BinanceService = serviceProvider.GetRequiredService<IMainSettingsStocks>();
BinanceService.Init();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
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