using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UlrShortener.Models;
using UlrShortener.Services;

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
    
    builder.Services.AddScoped<UrlShorteningService>();
    builder.Services.AddMemoryCache();
}
var app = builder.Build();

app.MapPost("/shorten", async (ShortenUrlRequest request, UrlShorteningService service, HttpContext context) => 
{
    if(!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
        return Results.BadRequest("The specified URL is invalid");

    var shortUrl = await service.CreateShortenedUrl(request.Url, context);
    return Results.Ok(shortUrl);
});

app.MapGet("/{code}", async (string code, UrlShorteningService service) => 
{
    var longUrl = await service.GetLongUrl(code);
    return longUrl is not null ? Results.Ok(longUrl) : Results.NotFound();
});

app.Run();

public record ShortenUrlRequest(string Url);