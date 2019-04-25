using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;

namespace Forte.RedirectMiddleware.Repository.ResolverRepository
{
    public interface IRedirectRuleResolverRepository
    {
        RedirectRule FindByOldPath(UrlPath oldPath);
    }
    public abstract class RedirectRuleResolverRepository : IRedirectRuleResolverRepository
    {
        public abstract RedirectRule FindByOldPath(UrlPath oldPath);
    }
} 