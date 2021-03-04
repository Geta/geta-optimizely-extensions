using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EPiServer.Core;

[assembly: InternalsVisibleTo("FakeMaker.Commerce")]

namespace Geta.EPi.Extensions.Tests.EPiFakeMaker
{
    public abstract class Fake : IFake
    {
        public abstract IList<IFake> Children { get; }
        public abstract IContent Content { get; protected set; }

        internal abstract void HelpCreatingMockForCurrentType(IFakeMaker maker);
    }
}
