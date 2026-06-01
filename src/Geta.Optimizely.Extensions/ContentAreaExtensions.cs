using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.ServiceLocation;

namespace Geta.Optimizely.Extensions
{
    /// <summary>
    /// Content area extensions.
    /// </summary>
    public static class ContentAreaExtensions
    {
        /// <summary>
        /// Checks if content area has any content that is visible to the current user
        /// (published, not expired and not access restricted).
        /// </summary>
        /// <param name="contentArea">The content area.</param>
        /// <returns>Returns true if the content area has at least one visible item and false when not.</returns>
        public static bool HasContent(this ContentArea contentArea)
        {
            if (contentArea?.Items == null)
            {
                return false;
            }

            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var publishedStateAssessor = ServiceLocator.Current.GetInstance<IPublishedStateAssessor>();
            var accessEvaluator = ServiceLocator.Current.GetInstance<IContentAccessEvaluator>();
            var principal = ServiceLocator.Current.GetInstance<IPrincipalAccessor>().Principal;

            return contentArea.Items.Any(item => IsVisible(item, contentLoader, publishedStateAssessor, accessEvaluator, principal));
        }

        // CMS 13 removed ContentArea.FilteredItems. This replicates its visibility filtering
        // (publish state, expiration and read access) so HasContent does not report items
        // that would not actually be rendered for the current user.
        private static bool IsVisible(
            ContentAreaItem item,
            IContentLoader contentLoader,
            IPublishedStateAssessor publishedStateAssessor,
            IContentAccessEvaluator accessEvaluator,
            System.Security.Principal.IPrincipal principal)
        {
            if (item?.ContentLink == null || !contentLoader.TryGet<IContent>(item.ContentLink, out var content))
            {
                return false;
            }

            return publishedStateAssessor.IsPublished(content)
                   && accessEvaluator.HasAccess(content, principal, AccessLevel.Read);
        }
    }
}
