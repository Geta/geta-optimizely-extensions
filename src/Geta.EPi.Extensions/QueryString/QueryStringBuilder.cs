using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Geta.Optimizely.Extensions.Helpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Web;

namespace Geta.Optimizely.Extensions.QueryString
{
    /// <summary>
    ///     Helper class for creating and modifying URL's QueryString.
    /// </summary>
    public class QueryStringBuilder : IHtmlContent
    {
        protected readonly UrlBuilder UrlBuilder;
        protected readonly UrlResolver EPiUrlResolver;

        /// <summary>
        ///     Represents the empty query string. Field is read-only.
        /// </summary>
        public static readonly QueryStringBuilder Empty = new QueryStringBuilder(string.Empty);

        /// <summary>
        ///     Instantiates new QueryStringBuilder with provided URL.
        /// </summary>
        /// <param name="url">URL for which to build query.</param>
        public QueryStringBuilder(string url)
        {
            UrlBuilder = new UrlBuilder(url);
        }

        /// <summary>
        ///     Instantiates new QueryStringBuilder with provided URL.
        /// </summary>
        /// <param name="contentLink">ContentReference for which to build query.</param>
        /// <param name="includeHost">Mark if include host name in the url.</param>
        public QueryStringBuilder(ContentReference contentLink, bool includeHost = false) : this(contentLink, ServiceLocator.Current.GetInstance<UrlResolver>(), includeHost)
        {
        }

        /// <summary>
        ///     Instantiates new QueryStringBuilder with provided URL.
        /// </summary>
        /// <param name="contentLink">ContentReference for which to build query.</param>
        /// <param name="urlResolver">UrlResolver instance.</param>
        /// <param name="includeHost">Mark if include host name in the url.</param>
        /// <param name="context">HttpContext, include if includeHost is true.</param>
        public QueryStringBuilder(ContentReference contentLink, UrlResolver urlResolver, bool includeHost = false, HttpContext context = null)
        {
            var url = contentLink.GetFriendlyUrl(includeHost, false, context, urlResolver);

            UrlBuilder = new UrlBuilder(url);
        }

        /// <summary>
        ///     Factory method for instantiating new QueryStringBuilder with provided URL.
        /// </summary>
        /// <param name="url">URL for which to build query.</param>
        /// <returns>Instance of QueryStringBuilder.</returns>
        public static QueryStringBuilder Create(string url)
        {
            return new QueryStringBuilder(url);
        }

        /// <summary>
        ///     Factory method for instantiating new QueryStringBuilder with provided URL.
        /// </summary>
        /// <param name="contentLink">Content for which to build query.</param>
        /// <param name="includeHost">Mark if include host name in the url.</param>
        /// <returns>Instance of QueryStringBuilder.</returns>
        public static QueryStringBuilder Create(ContentReference contentLink, bool includeHost = false)
        {
            return new QueryStringBuilder(contentLink, includeHost);
        }

        /// <summary>
        ///     Factory method for instantiating new QueryStringBuilder with provided URL.
        /// </summary>
        /// <param name="contentLink">Content for which to build query.</param>
        /// <returns>Instance of QueryStringBuilder.</returns>
        /// <param name="urlResolver">UrlResolver instance.</param>
        /// <param name="includeHost">Mark if include host name in the url.</param>
        public static QueryStringBuilder Create(ContentReference contentLink, UrlResolver urlResolver, bool includeHost = false)
        {
            return new QueryStringBuilder(contentLink, urlResolver, includeHost);
        }

        /// <summary>
        ///     Adds query string parameter to query URL encoded.
        /// </summary>
        /// <param name="name">Name of parameter.</param>
        /// <param name="value">Value of parameter.</param>
        /// <returns>Instance of modified QueryStringBuilder.</returns>
        public QueryStringBuilder Add(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                UrlBuilder.QueryCollection[name] = HttpUtility.UrlEncode(value);
            }

            return this;
        }

        /// <summary>
        ///     Adds query string parameter to query URL encoded.
        /// </summary>
        /// <param name="name">Name of parameter.</param>
        /// <param name="value">Value of parameter.</param>
        /// <returns>Instance of modified QueryStringBuilder.</returns>
        public QueryStringBuilder Add(string name, object value)
        {
            if (value == null)
            {
                return this;
            }

            return Add(name, value.ToString());
        }

        /// <summary>
        ///     Adds a segment at the end of the URL.
        /// </summary>
        /// <param name="segment">Name of the segment</param>
        /// <returns>Instance of modified QueryStringBuilder.</returns>
        public QueryStringBuilder AddSegment(string segment)
        {
            UrlBuilder.Path = UrlBuilder.Path.AppendTrailingSlash() + HttpUtility.UrlPathEncode(segment.TrimStart('/'));
            return this;
        }

        /// <summary>
        ///     Removes query string parameter from query.
        /// </summary>
        /// <param name="name">Name of parameter to remove.</param>
        /// <returns>Instance of modified QueryStringBuilder.</returns>
        public QueryStringBuilder Remove(string name)
        {
            UrlBuilder.QueryCollection.Remove(name);
            return this;
        }

        /// <summary>
        ///     Adds query string parameter to query string if it is not already present, otherwise it removes it.
        /// </summary>
        /// <param name="name">Name of parameter to add or remove.</param>
        /// <param name="value">Value of parameter to add.</param>
        /// <returns>Instance of modified QueryStringBuilder.</returns>
        public QueryStringBuilder Toggle(string name, string value)
        {
            var currVal = HttpUtility.UrlDecode(UrlBuilder.QueryCollection[name]);
            var exists = currVal != null && currVal == value;

            if (exists)
                Remove(name);
            else
                Add(name, value);

            return this;
        }

        /// <summary>
        ///     Returns string representation of URL with query string.
        /// </summary>
        /// <returns>String representation of URL with query string.</returns>
        public override string ToString()
        {
            return UrlBuilder.ToString();
        }

        /// <inheritdoc />
        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            encoder.Encode(writer, UrlBuilder.ToString());
        }

        /// <summary>
        ///     Returns string representation of URL with query string. This is implementation of IHtmlContent.
        /// </summary>
        /// <returns>String representation of URL with query string.</returns>
        public string ToHtmlString()
        {
            return ToString();
        }
    }
}