namespace UlrShortener.Models;

public class ShortUrl
{
    public Guid Id {get;set;} 
    public required string Url {get;set;}
    public required string Code {get;set;}
    public DateTime CreatedOn {get;set;}
}