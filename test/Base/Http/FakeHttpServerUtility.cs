using System;

namespace BVNetwork.NotFound.Tests.Base.Http
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