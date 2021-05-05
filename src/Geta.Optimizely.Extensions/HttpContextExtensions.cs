using EPiServer.Web;
using Geta.Optimizely.Extensions.Helpers;
using Microsoft.AspNetCore.Http;
using System;

namespace Geta.Optimizely.Extensions
{
    /// <summary>
    /// Extensions for <see cref="HttpContext"/>
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Returns base URI for the site.
        /// </summary>
        /// <returns>Base site URI</returns>
        public static Uri GetBaseUri(this HttpContext httpContext)
        {
            return httpContext != null ? UriHelpers.GetBaseUri(httpContext, SiteDefinition.Current) : null;
        }

        /// <summary>
        ///     Checks HttpContextBase.Items for a key named IsBlockPreviewTemplate and if it's set to true.
        ///     This key is set through the TemplateResolver.TemplateResolved event in the <see cref="ExtensionsInitializationModule">initialization module</see>
        /// </summary>
        /// <param name="httpContext">The current HttpContextBase instance</param>
        /// <returns>True/false</returns>
        public static bool IsBlockPreview(this HttpContext httpContext)
        {
            object isBlockPreview = httpContext?.Items["IsBlockPreviewTemplate"];

            if (isBlockPreview != null && (bool)isBlockPreview)
            {
                return true;
            }

            return false;
        }
    }
}