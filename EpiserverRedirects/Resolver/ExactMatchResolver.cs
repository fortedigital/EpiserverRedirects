using System.Linq;
using System.Threading.Tasks;
using EPiServer;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;

namespace Forte.EpiserverRedirects.Resolver
{
    public class ExactMatchResolver : BaseRuleResolver, IRedirectRuleResolver
    {
        private readonly IQueryable<RedirectRule> _redirectRuleResolverRepository;

        public ExactMatchResolver(IQueryable<RedirectRule> redirectRuleResolverRepository, IContentLoader contentLoader) : base(contentLoader)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath)
        {
            return Task.Run(() =>
            {
                var rule = _redirectRuleResolverRepository
                    .Where(r => r.IsActive && r.RedirectRuleType == RedirectRuleType.ExactMatch)
                    .OrderBy(x => x.Priority)
                    .FirstOrDefault(r => r.OldPattern == oldPath.ToString());

                return ResolveRule(rule, r => new ExactMatchRedirect(r));
            });
        }
    }
}
