using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;
using Forte.RedirectMiddleware.Repository.ResolverRepository;

namespace Forte.RedirectMiddleware.Resolver
{
    public interface IRedirectRuleResolver
    {
        RedirectRule ResolveRedirectRule(UrlPath oldPath);
    }
    
    [ServiceConfiguration(ServiceType = typeof(IRedirectRuleResolver))]
    public class RedirectRuleResolver : IRedirectRuleResolver
    {
        private readonly IRedirectRuleResolverRepository _redirectRuleControllerRepository;

        public RedirectRuleResolver(IRedirectRuleResolverRepository redirectRuleControllerRepository)
        {
            _redirectRuleControllerRepository = redirectRuleControllerRepository;
        }

        public RedirectRule ResolveRedirectRule(UrlPath oldPath)
        {
            return _redirectRuleControllerRepository.FindByOldPath(oldPath);
        }
        
    }
}