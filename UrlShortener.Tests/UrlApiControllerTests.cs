using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.Controllers;
using UrlShortener.Models.DTO;
using UrlShortener.Models.Entities;
using UrlShortener.Services;

namespace UrlShortener.Tests
{
    public class UrlApiControllerTests
    {
        [Fact]
        public async Task CreateShortUrl_ReturnsOk_WithShortUrl()
        {
            // Arrange
            var mockService = new Mock<IUrlService>();
            var longUrl = "https://example.com";
            var shortCode = "abc12345";
            mockService.Setup(s => s.CreateShortUrlAsync(longUrl))
                       .ReturnsAsync(new ShortUrl(longUrl, shortCode));

            var controller = new UrlApiController(mockService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            controller.ControllerContext.HttpContext.Request.Scheme = "https";
            controller.ControllerContext.HttpContext.Request.Host = new HostString("localhost", 5001);

            var request = new CreateShortUrlRequest { LongUrl = longUrl };

            // Act
            var result = await controller.CreateShortUrl(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Contains(shortCode, result.Value?.ToString());
        }

        [Fact]
        public async Task CreateShortUrl_ReturnsBadRequest_ForInvalidUrl()
        {
            // Arrange
            var mockService = new Mock<IUrlService>();
            var controller = new UrlApiController(mockService.Object);

            var invalidUrlRequest = new CreateShortUrlRequest
            {
                LongUrl = "not-a-url"
            };

            // Act
            var result = await controller.CreateShortUrl(invalidUrlRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task CreateShortUrl_ReturnsBadRequest_ForNonHttpOrHttpsScheme()
        {
            // Arrange
            var mockService = new Mock<IUrlService>();
            var controller = new UrlApiController(mockService.Object);

            var invalidUrlRequest = new CreateShortUrlRequest
            {
                LongUrl = "ftp://example.com/resource"
            };

            // Act
            var result = await controller.CreateShortUrl(invalidUrlRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Contains("Only 'http' and 'https'", badRequestResult.Value?.ToString());
        }
    }
}
