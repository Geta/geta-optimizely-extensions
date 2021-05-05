using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using Geta.Net.Extensions;

namespace Geta.Optimizely.Extensions
{
    /// <summary>
    ///     LinkItemCollection extensions.
    /// </summary>
    public static class LinkItemCollectionExtensions
    {
        /// <summary>
        ///     Returns a PageDataCollection with all the Optimizely pages from a LinkItemCollection.
        /// </summary>
        /// <param name="linkItemCollection">Source LinkItemCollection to look for Optimizely pages.</param>
        /// <returns>PageDataCollection with EPiServer pages from a LinkItemCollection.</returns>
        public static PageDataCollection ToPageDataCollection(this LinkItemCollection linkItemCollection)
        {
            return linkItemCollection.ToEnumerable<PageData>().ToPageDataCollection();
        }

        /// <summary>
        ///     Returns a sequence with all the Optimizely pages of given type <typeparamref name="T" /> in a LinkItemCollection
        /// </summary>
        /// <param name="linkItemCollection">Source LinkItemCollection to look for Optimizely pages.</param>
        /// <returns>Sequence of the Optimizely pages of type <typeparamref name="T" /> in a LinkItemCollection</returns>
        public static IEnumerable<T> ToEnumerable<T>(this LinkItemCollection linkItemCollection)
            where T : PageData
        {
            if (linkItemCollection == null)
            {
                return Enumerable.Empty<T>();
            }

            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            return linkItemCollection
                .Select(x => x.ToContentReference())
                .Where(x => !x.IsNullOrEmpty())
                .Select(contentLoader.Get<IContent>)
                .SafeOfType<T>();
        }
    }
}