using Microsoft.EntityFrameworkCore;
using UlrShortener.Models;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Configuration.Sources.Clear();
    builder.Configuration.AddJsonFile("appsettings.json", optional: true);
    if(builder.Environment.IsDevelopment())
    {
        builder.Configuration.AddUserSecrets<Program>();
    }
    var connString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<AppDbContext>(options => 
        options.UseNpgsql(connString));
}
var app = builder.Build();



app.MapGet("/", () => "Hello World!");

app.Run();
