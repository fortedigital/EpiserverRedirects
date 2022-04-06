using System;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Redirect
{
    [Obsolete]
    public class WildcardRedirect : Redirect
    {
        public WildcardRedirect(RedirectRule redirectRule) : base(redirectRule)
        {
        }

        protected override string GetPathWithoutContentId(Uri request, bool shouldPreserveQueryString)
        {
            var newUrl = "   ";

            return newUrl;
        }
    }
}