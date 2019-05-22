using System;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Redirect
{
    public class WildcardRedirect : Redirect
    {
        public WildcardRedirect(RedirectRule redirectRule) : base(redirectRule)
        {
        }

        protected override string GetPathWithoutContentId(Uri request)
        {
            var newUrl = "   ";

            return newUrl;
        }
    }
}