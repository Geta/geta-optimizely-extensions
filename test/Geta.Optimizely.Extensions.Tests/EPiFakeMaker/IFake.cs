using System.Collections.Generic;
using EPiServer.Core;

namespace Geta.Optimizely.Extensions.Tests.EPiFakeMaker
{
    public interface IFake
    {
        IContent Content { get; }
        IList<IFake> Children { get; }
    }
}
