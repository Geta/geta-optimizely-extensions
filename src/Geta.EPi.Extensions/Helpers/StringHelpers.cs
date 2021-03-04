namespace Geta.EPi.Extensions.Helpers
{
    internal static class StringHelpers
    {
        internal static string AppendTrailingSlash(this string path)
        {
            if (path == null)
                return null;
            var length = path.Length;
            if (length == 0 || path[length - 1] == '/')
                return path;
            path += "/";
            return path;
        }
    }
}
