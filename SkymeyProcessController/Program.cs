var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();
builder.Configuration.AddJsonFile("F:\\Skymey\\SkymeyAPI\\SkymeyUserService\\appsettingsUserService.json");
builder.WebHost.UseUrls("http://localhost:5010;https://localhost:5011;");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapRazorPages();
app.UseAuthorization();

app.MapControllers();

app.Run();
