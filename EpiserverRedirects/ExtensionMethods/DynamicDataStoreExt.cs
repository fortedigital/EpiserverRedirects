using System;
using EPiServer.Data;
using EPiServer.Data.Dynamic;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.ExtensionMethods
{
    public static class DynamicDataStoreExt
    {
        public static Identity SaveWithEncodedUrls(this DynamicDataStore d, RedirectRule redirectRule)
        {
            if (redirectRule != null)
            {
                var oldPattern = redirectRule.OldPattern;
                var newPattern = redirectRule.NewPattern;

                redirectRule.OldPattern = oldPattern != null && !oldPattern.Contains("%")
                    ? Uri.EscapeUriString(oldPattern)
                    : oldPattern;
                
                redirectRule.NewPattern = newPattern != null && !newPattern.Contains("%")
                    ? Uri.EscapeUriString(newPattern)
                    : newPattern;
            }

            return d.Save(redirectRule);
        }
    }
}