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

    public async Task<string> GenerateUniqueUrl()
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
}