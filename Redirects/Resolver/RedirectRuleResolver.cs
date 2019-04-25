using System.Linq;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;

namespace Forte.RedirectMiddleware.Resolver
{
    public interface IRedirectRuleResolver
    {
        RedirectRule ResolveRedirectRule(UrlPath oldPath);
    }
    
    [ServiceConfiguration(ServiceType = typeof(IRedirectRuleResolver))]
    public class RedirectRuleResolver : IRedirectRuleResolver
    {
        private readonly IQueryable<RedirectRule> _redirectRuleResolverRepository;

        public RedirectRuleResolver(IQueryable<RedirectRule> redirectRuleResolverRepository)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public RedirectRule ResolveRedirectRule(UrlPath oldPath)
        {
            return _redirectRuleResolverRepository.FirstOrDefault(r=>r.OldPath == oldPath);
        }
        
    }
}