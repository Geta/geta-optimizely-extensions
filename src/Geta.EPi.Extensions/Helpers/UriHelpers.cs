using EPiServer;
using EPiServer.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;

namespace Geta.EPi.Extensions.Helpers
{
    /// <summary>
    /// Uri helpers
    /// </summary>
    public class UriHelpers
    {
        /// <summary>
        /// Returns base URI for the site.
        /// </summary>
        /// <returns>Base site URI</returns>
        public static Uri GetBaseUri(HttpContext context, SiteDefinition siteDefinition)
        {
            var siteUri = context != null
                ? context.Request.GetDisplayUrl()
                : siteDefinition.SiteUrl.ToString();

            var scheme = context != null && !string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-Proto"])
                ? context.Request.Headers["X-Forwarded-Proto"].ToString().Split(',')[0]
                : context != null ? context.Request.Scheme : siteDefinition.SiteUrl.Scheme;

            var urlBuilder = new UrlBuilder(siteUri)
            {
                Scheme = scheme ?? "https"
            };
            return urlBuilder.Uri;
        }
    }
}