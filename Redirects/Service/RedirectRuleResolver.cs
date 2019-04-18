using System;
using System.Linq;
using EPiServer.ServiceLocation;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Repository;

namespace Forte.RedirectMiddleware.Service
{
    [ServiceConfiguration(ServiceType = typeof(IRedirectRuleResolver))]
    public class RedirectRuleResolver : IRedirectRuleResolver
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;

        public RedirectRuleResolver(IRedirectRuleRepository redirectRuleRepository)
        {
            _redirectRuleRepository = redirectRuleRepository;
        }

        public RedirectRule ResolveRedirectRule(UrlPath oldPath)
        {
            return _redirectRuleRepository.GetRedirectRule(oldPath);
        }
        
    }

    public interface IRedirectRuleResolver
    {
        RedirectRule ResolveRedirectRule(UrlPath oldPath);
    }
}