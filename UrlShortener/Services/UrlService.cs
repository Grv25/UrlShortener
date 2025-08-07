using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models.Entities;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly UrlDbContext _context;

        public UrlService(UrlDbContext context)
        {
            _context = context;
        }

        public async Task<ShortUrl> CreateShortUrlAsync(string longUrl)
        {
            var existing = await _context.ShortUrls.FirstOrDefaultAsync(u => u.LongUrl == longUrl);
            if (existing != null)
            {
                return existing;
            }

            var code = Guid.NewGuid().ToString()[..8];
            var shortUrl = new ShortUrl(longUrl, code);

            _context.ShortUrls.Add(shortUrl);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19)
            {
                var alreadyExists = await _context.ShortUrls.FirstOrDefaultAsync(u => u.LongUrl == longUrl);
                if (alreadyExists != null)
                    return alreadyExists;

                throw;
            }

            return shortUrl;
        }

        public async Task<string?> GetLongUrlAsync(string shortCode)
        {
            var foundUrl = await _context.ShortUrls.FirstOrDefaultAsync(x => x.ShortCode == shortCode);
            return foundUrl?.LongUrl;
        }
    }
}
