using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener
{
    public class UrlDbContext : DbContext
    {
        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options) { }
        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}
