using Microsoft.AspNetCore.Http;

namespace Geta.Optimizely.Extensions
{
    /// <summary>
    /// HttpRequestBase extensions.
    /// </summary>
    public static class RequestBaseExtensionMethods
    {
        /// <summary>
        /// Returns user IP address.
        /// </summary>
        /// <param name="requestBase"></param>
        /// <returns></returns>
        public static string GetUserIp(this HttpRequest requestBase)
        {
            if (requestBase == null)
            {
                return string.Empty;
            }

            //First get HTTP_X_FORWARDED_FOR ip if client is behind a proxy
            var userIp = requestBase.HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR");

            if (string.IsNullOrEmpty(userIp))
            {
                return requestBase.HttpContext.GetServerVariable("REMOTE_ADDR");
            }

            var ipArray = userIp.Split(',');
            return ipArray[0];
        }
    }
}