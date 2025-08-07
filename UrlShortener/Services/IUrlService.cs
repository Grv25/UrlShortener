using UrlShortener.Models.Entities;

namespace UrlShortener.Services
{
    public interface IUrlService
    {
        Task<ShortUrl> CreateShortUrlAsync(string longUrl);
        Task<string?> GetLongUrlAsync(string shortCode);
    }
}
