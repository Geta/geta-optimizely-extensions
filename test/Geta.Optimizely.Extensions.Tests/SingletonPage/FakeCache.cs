using System.Collections.Concurrent;
using EPiServer.Core;
using Geta.Optimizely.Extensions.SingletonPage;

namespace Geta.Optimizely.Extensions.Tests.SingletonPage
{
    public class FakeCache : DefaultContentReferenceCache
    {
        public ConcurrentDictionary<CacheKey, ContentReference> InternalCache => Cache;
    }
}