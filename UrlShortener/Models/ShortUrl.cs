namespace UrlShortener.Models
{
    public class ShortUrl
    {
        public int Id { get; set; }
        public string LongUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;

        public ShortUrl(string longUrl, string shortCode)
        {
            LongUrl = longUrl;
            ShortCode = shortCode;
        }
    }
}
