using System.Globalization;
using EPiServer;
using EPiServer.Core;

namespace Forte.Redirects.System
{
    public enum SystemRedirectReason
    {
        PublishedContent,
        MovedContent,
        SavedContent
    }
    
    public static class SystemRedirectsHelper
    {
        public static string Combine(string str1, string str2)
        {
            str1 = str1.TrimEnd('/');
            str2 = str2.TrimStart('/');
            return $"{str1}/{str2}";
        }

        public static CultureInfo GetCultureInfo(ContentEventArgs e)
        {
            var localizable = e.Content as ILocalizable;
            return localizable?.Language;
        }

        public static string GetSystemRedirectReason(SystemRedirectReason systemRedirectReason)
        {
            switch (systemRedirectReason)
            {
                case SystemRedirectReason.PublishedContent:
                    return "Generated from publishing new content event";
                case SystemRedirectReason.MovedContent:
                    return "Generated from moving exiting content event";
                case SystemRedirectReason.SavedContent:
                    return "Generated from saving exiting content event";
            }

            return null;
        }
    }
}