using System;
using System.Text.RegularExpressions;
using Forte.EpiserverRedirects.Extensions;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Redirect
{
    public class RegexRedirect : Redirect
    {
        public RegexRedirect(IRedirectRule redirectRule) : base(redirectRule)
        {
        }

        protected override string GetPathWithoutContentId(Uri request, bool shouldPreserveQueryString)
        {
            return Regex.Replace(request.AbsolutePath, RedirectRule.OldPattern.ToStrictRegexPattern(),
                RedirectRule.NewPattern, RegexOptions.IgnoreCase);
        }
    }
}