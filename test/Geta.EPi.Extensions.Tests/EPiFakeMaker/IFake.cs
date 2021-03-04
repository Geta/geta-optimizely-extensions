using System.Collections.Generic;
using EPiServer.Core;

namespace Geta.EPi.Extensions.Tests.EPiFakeMaker
{
    public interface IFake
    {
        IContent Content { get; }
        IList<IFake> Children { get; }
    }
}
