namespace Forte.EpiserverRedirects.UrlRewritePlugin
{
    public static class StringExtensions
    {
        public static string NormalizePath(this string str)
        {
            return '/' + str.Trim('/');
        }
    }
}