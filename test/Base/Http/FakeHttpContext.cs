using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Geta.EPi.Extensions.Tests.Base.Http
{
    public class FakeHttpContext : HttpContext
    {
        public FakeHttpContext(FakeHttpRequest fakeHttpRequest)
        {
            Request = fakeHttpRequest;
        }

        public override void Abort()
        {
            throw new NotImplementedException();
        }

        public override IFeatureCollection Features { get; }
        public override HttpRequest Request { get; } = new FakeHttpRequest();
        public override HttpResponse Response { get; } = new FakeHttpResponse();
        public override ConnectionInfo Connection { get; }
        public override WebSocketManager WebSockets { get; }
        public override ClaimsPrincipal User { get; set; }
        public override IDictionary<object, object> Items { get; set; } = new Dictionary<object, object>();
        public override IServiceProvider RequestServices { get; set; }
        public override CancellationToken RequestAborted { get; set; }
        public override string TraceIdentifier { get; set; }
        public override ISession Session { get; set; }

        public static FakeHttpContext CreateWithRequest(FakeHttpRequest fakeHttpRequest)
        {
            return new FakeHttpContext(fakeHttpRequest);
        }
    }
}