namespace Forte.EpiserverRedirects.UrlRewritePlugin
{
    public static class StringExtensions
    {
        public static string NormalizePath(this string str)
        {
            str = str[0] != '/' ? '/' + str : str;
            str = str.TrimEnd('/');
            
            return str;
        }
    }
}