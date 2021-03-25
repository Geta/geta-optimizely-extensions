using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Geta.EPi.Extensions.Tests.Base.Http
{
    public class FakeHttpRequest : HttpRequest
    {
        private const string QueryStringVariable = "QUERY_STRING";

        private static Uri _uri = new Uri("http://example.com");
        public NameValueCollection ServerVariables { get; } = new NameValueCollection();

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public override HttpContext HttpContext { get; }
        public override string Method { get; set; }
        public override string Scheme { get; set; } = _uri.Scheme;
        public override bool IsHttps { get; set; }
        public override HostString Host { get; set; } = new HostString(_uri.Host);
        public override PathString PathBase { get; set; }
        public override PathString Path { get; set; }
        public override Microsoft.AspNetCore.Http.QueryString QueryString { get; set; }
        public override IQueryCollection Query { get; set; }
        public override string Protocol { get; set; }
        public override IHeaderDictionary Headers { get; } = new HeaderDictionary();
        public override IRequestCookieCollection Cookies { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }
        public override Stream Body { get; set; }
        public override bool HasFormContentType { get; }
        public override IFormCollection Form { get; set; }

        public FakeHttpRequest WithQueryString(string value)
        {
            ServerVariables.Add(QueryStringVariable, value);
            return this;
        }

        public FakeHttpRequest WithUrl(string expected)
        {
            _uri = string.IsNullOrEmpty(expected) ? null : new Uri(expected);
            Scheme = _uri.Scheme;
            Host = new HostString(_uri.Host);
            return this;
        }

        public FakeHttpRequest WithHeader(string key, string value)
        {
            Headers.Add(key, value);
            return this;
        }
    }
}