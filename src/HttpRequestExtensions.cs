using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Geta.EPi.Extensions
{
    /// <summary>
    /// HttpRequest extensions.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Checks if current request is a crawler request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsCrawler(this HttpRequest request)
        {
            var userAgent = request.Headers["User-Agent"].ToString();

            if (string.IsNullOrWhiteSpace(userAgent))
                return false;

            return Regex.IsMatch(
                userAgent,
                @"bot|crawler|baiduspider|80legs|ia_archiver|voyager|curl|wget|yahoo! slurp|mediapartners-google",
                RegexOptions.IgnoreCase);
        }
    }
}
