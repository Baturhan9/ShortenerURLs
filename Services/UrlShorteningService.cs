using Microsoft.EntityFrameworkCore;
using UlrShortener.Models;

namespace UlrShortener.Services;

public class UrlShorteningService
{
    private readonly AppDbContext _context;
    private readonly Random _rand = new Random();
    public UrlShorteningService(AppDbContext context)
    {
        _context = context;
    }

    private async Task<string> GenerateUniqueUrl()
    {
        var codeChars = new char[ShortLinkSettings.Length];
        int maxValue = ShortLinkSettings.Alphabet.Length;

        while(true)
        {
            for(int i = 0; i < ShortLinkSettings.Length; i++)
            {
                int randomIndex = _rand.Next(maxValue);
                codeChars[i] = ShortLinkSettings.Alphabet[randomIndex];
            }

            string code = new string(codeChars);

            if(!await _context.ShortenedUrls.AnyAsync(s => s.Code == code))
                return code;
        }
    }

    public async Task<string> CreateShortenedUrl(string LongUrl, HttpContext httpContext)
    {
        var code = await GenerateUniqueUrl();
        var request = httpContext.Request;
        var shortenedUrl = new ShortenedUrl()
        {
            Id = Guid.NewGuid(),
            LongUrl = LongUrl,
            Code = code,
            ShortUrl = $"{request.Scheme}://{request.Host}/{code}",
            CreatedOn = DateTime.UtcNow
        };
        _context.ShortenedUrls.Add(shortenedUrl);
        await _context.SaveChangesAsync();
        return shortenedUrl.ShortUrl;
    }

    public async Task<string?> GetLongUrl(string code)
    {
        var shortenedUrl = await _context.ShortenedUrls.SingleOrDefaultAsync(u => u.Code == code);
        return shortenedUrl?.LongUrl ?? null;
    }

}