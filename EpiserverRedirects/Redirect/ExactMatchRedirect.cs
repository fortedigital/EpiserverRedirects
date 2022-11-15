using System;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Redirect
{
    public class ExactMatchRedirect : Redirect
    {
        public ExactMatchRedirect(RedirectRuleModel redirectRule) : base(redirectRule)
        {
        }

        protected override string GetPathWithoutContentId(Uri request, bool preserveQueryString)
        {
            var newUrl = RedirectRule.NewPattern;

            return preserveQueryString ? newUrl + request.Query : newUrl;
        }
    }
}