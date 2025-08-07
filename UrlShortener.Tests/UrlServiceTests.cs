using Microsoft.EntityFrameworkCore;
using UrlShortener.Services;

namespace UrlShortener.Tests
{
    public class UrlServiceTests
    {
        private static UrlDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<UrlDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new UrlDbContext(options);
        }

        [Fact]
        public async Task CreateShortUrlAsync_ShouldCreateNewShortUrl()
        {
            // Arrange
            var context = GetDbContext();
            var service = new UrlService(context);
            var longUrl = "https://example.com";

            // Act
            var result = await service.CreateShortUrlAsync(longUrl);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(longUrl, result.LongUrl);
            Assert.False(string.IsNullOrEmpty(result.ShortCode));
        }

        [Fact]
        public async Task CreateShortUrlAsync_ShouldReturnExistingShortUrl_WhenUrlAlreadyExists()
        {
            // Arrange
            var context = GetDbContext();
            var service = new UrlService(context);
            var longUrl = "https://example.com";

            var first = await service.CreateShortUrlAsync(longUrl);

            // Act
            var second = await service.CreateShortUrlAsync(longUrl);

            // Assert
            Assert.Equal(first.Id, second.Id);
            Assert.Equal(first.ShortCode, second.ShortCode);
        }

        [Fact]
        public async Task GetLongUrlAsync_ShouldReturnLongUrl_WhenShortCodeExists()
        {
            // Arrange
            var context = GetDbContext();
            var service = new UrlService(context);
            var longUrl = "https://example.com";
            var shortUrl = await service.CreateShortUrlAsync(longUrl);

            // Act
            var result = await service.GetLongUrlAsync(shortUrl.ShortCode);

            // Assert
            Assert.Equal(longUrl, result);
        }

        [Fact]
        public async Task GetLongUrlAsync_ShouldReturnNull_WhenShortCodeDoesNotExist()
        {
            // Arrange
            var context = GetDbContext();
            var service = new UrlService(context);

            // Act
            var result = await service.GetLongUrlAsync("nonexistent");

            // Assert
            Assert.Null(result);
        }
    }
}
