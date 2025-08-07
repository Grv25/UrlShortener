using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.Controllers;
using UrlShortener.Services;

namespace UrlShortener.Tests
{
    public class RedirectControllerTests
    {
        [Fact]
        public async Task RedirectToLongUrl_ReturnsRedirect_WhenFound()
        {
            // Arrange
            var shortCode = "abc12345";
            var longUrl = "https://example.com";

            var mockService = new Mock<IUrlService>();
            mockService.Setup(s => s.GetLongUrlAsync(shortCode))
                       .ReturnsAsync(longUrl);

            var controller = new RedirectController(mockService.Object);

            // Act
            var result = await controller.RedirectToLongUrl(shortCode);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(longUrl, redirectResult.Url);
        }

        [Fact]
        public async Task RedirectToLongUrl_ReturnsNotFound_WhenNotFound()
        {
            // Arrange
            var mockService = new Mock<IUrlService>();
            mockService.Setup(s => s.GetLongUrlAsync(It.IsAny<string>()))
                       .ReturnsAsync((string?)null);

            var controller = new RedirectController(mockService.Object);

            // Act
            var result = await controller.RedirectToLongUrl("nonexistent");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
