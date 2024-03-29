﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Geta.Optimizely.Extensions.Tests.Base.Http
{
    public class FakeHttpResponse : HttpResponse
    {
        public override void OnStarting(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }

        public override void OnCompleted(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }

        public override void Redirect(string location, bool permanent)
        {
            throw new NotImplementedException();
        }

        public override HttpContext HttpContext { get; }
        public override int StatusCode { get; set; }
        public override IHeaderDictionary Headers { get; }
        public override Stream Body { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }
        public override IResponseCookies Cookies { get; }
        public override bool HasStarted { get; }
        public string RedirectLocation { get; set; }

        public void End()
        {
        }

        public void RedirectPermanent(string url)
        {
            RedirectLocation = url;
            StatusCode = 301;
        }

        public void RedirectPermanent(string url, bool endResponse)
        {
            RedirectLocation = url;
            StatusCode = 301;
        }
    }
}