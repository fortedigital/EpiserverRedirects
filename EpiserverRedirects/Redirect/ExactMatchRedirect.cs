using System;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Redirect
{
    public class ExactMatchRedirect : Redirect
    {
        public ExactMatchRedirect(RedirectRule redirectRule) : base(redirectRule)
        {
        }

        protected override string GetPathWithoutContentId(Uri request)
        {
            var newUrl = RedirectRule.NewPattern;

            return newUrl;
        }
    }
}