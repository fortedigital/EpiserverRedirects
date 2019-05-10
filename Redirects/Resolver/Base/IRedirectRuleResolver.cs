using System.Threading.Tasks;
using Forte.RedirectMiddleware.Model.UrlPath;
using Forte.RedirectMiddleware.Redirect.Base;

namespace Forte.RedirectMiddleware.Resolver.Base
{
    public interface IRedirectRuleResolver
    {
        Task<IRedirect> ResolveRedirectRule(UrlPath oldPath);
    }

}