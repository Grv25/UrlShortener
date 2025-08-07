using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public RedirectController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        /// <summary>
        /// Перенаправляет по короткой ссылке
        /// </summary>
        /// <param name="shortCode">Код короткой ссылки</param>
        /// <returns>HTTP-редирект на длинный URL или 404</returns>
        /// <response code="302">Редирект на длинный URL</response>
        /// <response code="404">Ссылка не найдена</response>
        [HttpGet("/{shortCode}")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RedirectToLongUrl([FromRoute] string shortCode)
        {
            var longUrl = await _urlService.GetLongUrlAsync(shortCode);

            if (longUrl == null)
            {
                return NotFound("Short URL not found.");
            }

            return Redirect(longUrl);
        }
    }
}
