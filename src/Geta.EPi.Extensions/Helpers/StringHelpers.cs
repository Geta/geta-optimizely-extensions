using Geta.Net.Extensions;

namespace Geta.EPi.Extensions.Helpers
{
    internal static class StringHelpers
    {
        internal static string AppendTrailingSlash(this string path)
        {
            if (path.IsNullOrEmpty() || path.EndsWith('/'))
                return path;
            path += "/";
            return path;
        }
    }
}
