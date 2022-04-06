using System;
using System.Text.RegularExpressions;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Redirect
{
    public class RegexRedirect : Redirect
    {
        public RegexRedirect(RedirectRule redirectRule) : base(redirectRule)
        {
        }

        protected override string GetPathWithoutContentId(Uri request, bool shouldPreserveQueryString)
        {
            return Regex.Replace(request.ToString(), RedirectRule.OldPattern,
                RedirectRule.NewPattern, RegexOptions.IgnoreCase);
        }
    }
}