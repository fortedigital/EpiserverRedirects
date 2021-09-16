using System;
using System.Linq;
using System.Threading.Tasks;
using EPiServer;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.Resolver
{
    public class ExactMatchResolver : BaseRuleResolver, IRedirectRuleResolver
    {
        private readonly IRedirectRuleRepository _redirectRuleResolverRepository;

        public ExactMatchResolver(IRedirectRuleRepository redirectRuleResolverRepository, IContentLoader contentLoader) : base(contentLoader)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath)
        {
            return Task.Run(() =>
            {
                var encodedOldPath = Uri.EscapeUriString(oldPath.ToString());
                
                var rule = _redirectRuleResolverRepository
                    .GetAll()
                    .Where(r => r.IsActive && r.RedirectRuleType == RedirectRuleType.ExactMatch)
                    .OrderBy(x => x.Priority)
                    .FirstOrDefault(r => r.OldPattern == encodedOldPath);

                return ResolveRule(rule, r => new ExactMatchRedirect(r));
            });
        }
    }
}
