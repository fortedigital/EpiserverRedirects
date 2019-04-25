using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;
using Forte.RedirectMiddleware.Repository.ControllerRepository;

namespace Forte.RedirectMiddleware.Repository.ResolverRepository
{
    public class TestRedirectRuleResolverRepository : RedirectRuleResolverRepository
    {
        private readonly Dictionary<Guid, RedirectRule> _redirectsDictionary;

        public TestRedirectRuleResolverRepository()
        {
            _redirectsDictionary = new Dictionary<Guid, RedirectRule>();
        }
        public TestRedirectRuleResolverRepository(Dictionary<Guid, RedirectRule> redirectsCollection)
        {
            _redirectsDictionary = redirectsCollection;
        }

        public override RedirectRule FindByOldPath(UrlPath oldPath)
        {
            var redirect = _redirectsDictionary.FirstOrDefault(r => r.Value.OldPath == oldPath).Value;

            return redirect;
        }
    }
}