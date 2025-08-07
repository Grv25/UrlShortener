using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models.DTO;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlApiController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlApiController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        /// <summary>
        /// Создаёт короткую ссылку по длинному URL
        /// </summary>
        /// <param name="request">Модель с длинным URL</param>
        /// <returns>Сгенерированная короткая ссылка</returns>
        /// <response code="200">Короткая ссылка успешно создана</response>
        /// <response code="400">Невалидный формат URL</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateShortUrl([FromBody] CreateShortUrlRequest request)
        {
            if (!Uri.TryCreate(request.LongUrl, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                return BadRequest("Invalid URL. Only 'http' and 'https' schemes are allowed.");
            }

            var shortUrl = await _urlService.CreateShortUrlAsync(request.LongUrl);

            var uriBuilder = new UriBuilder
            {
                Scheme = Request.Scheme,
                Host = Request.Host.Host,
                Port = Request.Host.Port ?? (Request.Scheme == "https" ? 443 : 80),
                Path = shortUrl.ShortCode
            };

            var resultUrl = uriBuilder.Uri.ToString();

            return Ok(new { shortUrl = resultUrl });
        }
    }
}
