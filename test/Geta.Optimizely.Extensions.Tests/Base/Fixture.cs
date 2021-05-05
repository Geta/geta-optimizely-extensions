using System;
using System.Collections.Generic;
using Geta.Optimizely.Extensions.Tests.EPiFakeMaker;

namespace Geta.Optimizely.Extensions.Tests.Base
{
    public class Fixture
    {
        public static IEnumerable<FakePage> RandomPages(int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return FakePage.Create(Guid.NewGuid().ToString());
            }
        }
    }
}