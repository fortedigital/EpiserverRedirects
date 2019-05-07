using System.Threading.Tasks;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.UrlPath;

namespace Forte.RedirectMiddleware.Resolver
{
    public interface IRedirectRuleResolver
    {
        Task<RedirectRule> ResolveRedirectRule(UrlPath oldPath);
    }
}