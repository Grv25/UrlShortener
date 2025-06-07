using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly UrlDbContext _context;

        public UrlController(UrlDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateShortUrl([FromBody] string longUrl)
        {
            if (!Uri.IsWellFormedUriString(longUrl, UriKind.Absolute))
            {
                return BadRequest("Invalid URL format.");
            }                

            var code = Guid.NewGuid().ToString()[..8];
            var shortUrl = new ShortUrl(longUrl, code);

            _context.ShortUrls.Add(shortUrl);
            _context.SaveChanges();

            var resultUrl = $"{Request.Scheme}://{Request.Host}/{code}";
            return Ok(new { shortUrl = resultUrl });
        }

        [HttpGet("/{shortCode}")]
        public IActionResult RedirectToLongUrl(string shortCode)
        {
            var foundUrl = _context.ShortUrls.FirstOrDefault(x => x.ShortCode == shortCode);
            if (foundUrl == null)
            {
                return NotFound("Short URL not found.");
            }

            return Redirect(foundUrl.LongUrl);
        }
    }
}
