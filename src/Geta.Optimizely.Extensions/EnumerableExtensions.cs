using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;

namespace Geta.Optimizely.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool MemberOf(this IEnumerable<ContentReference> contentLinks, ContentReference contentReference)
        {
            if (contentLinks == null)
            {
                return false;
            }

            return contentLinks.Any(x => x.CompareToIgnoreWorkID(contentReference));
        }

        public static bool MemberOfAny(this IEnumerable<ContentReference> contentLinks, IEnumerable<ContentReference> otherContentLinks)
        {
            if (otherContentLinks == null || !otherContentLinks.Any())
            {
                return true;
            }

            if (contentLinks == null)
            {
                return false;
            }

            return otherContentLinks.Any(x => contentLinks.Any(y => y.CompareToIgnoreWorkID(x)));
        }

        public static bool MemberOfAll(this IEnumerable<ContentReference> contentLinks, IEnumerable<ContentReference> otherContentLinks)
        {
            if (otherContentLinks == null || !otherContentLinks.Any())
            {
                return true;
            }

            if (contentLinks == null)
            {
                return false;
            }

            return otherContentLinks.All(x => contentLinks.Any(y => y.CompareToIgnoreWorkID(x)));
        }
    }
}
