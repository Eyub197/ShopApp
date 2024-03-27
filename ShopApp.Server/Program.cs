using ShopApp.Server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
         .Build();
builder.Services.ConfigureAllServices(builder.Configuration);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
