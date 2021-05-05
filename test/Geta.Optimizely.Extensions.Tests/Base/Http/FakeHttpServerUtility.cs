using System;

namespace Geta.Optimizely.Extensions.Tests.Base.Http
{
    public class FakeHttpServerUtility
    {
        public Exception GetLastError()
        {
            return new Exception();
        }

        public void ClearError()
        {
        }
    }
}