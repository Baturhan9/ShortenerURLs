namespace UlrShortener.Models;

public class ShortenedUrl
{
    public Guid Id {get;set;} 
    public required string LongUrl {get;set;}
    public required string ShortUrl {get;set;}
    public required string Code {get;set;}
    public DateTime CreatedOn {get;set;}

}