namespace UrlShortener.Models.DTO
{
    public class CreateShortUrlRequest
    {
        /// <summary>
        /// Исходный длинный URL, который нужно сократить
        /// </summary>
        public string LongUrl { get; set; } = string.Empty;
    }
}
