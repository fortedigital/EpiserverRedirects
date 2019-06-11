using System;
using System.Linq;
using System.Threading.Tasks;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;

namespace Forte.EpiserverRedirects.Resolver
{
    [Obsolete]
    public class WildcardResolver : IRedirectRuleResolver
    {
        private readonly IQueryable<RedirectRule> _redirectRuleResolverRepository;

        public WildcardResolver(IQueryable<RedirectRule> redirectRuleResolverRepository)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath)
        {
            return Task.Run(() =>
            {
                var rule = _redirectRuleResolverRepository
                    .Where(r => r.IsActive && r.RedirectRuleType == RedirectRuleType.Wildcard)
                    .OrderBy(r => r.Priority)
                    .FirstOrDefault();

                var redirectRule = (rule != null) ? new WildcardRedirect(rule) : new NullRedirectRule() as IRedirect;

                return redirectRule;
            });
        }
        
    }
}