using System.Threading.Tasks;
using Forte.Redirects.Model.UrlPath;
using Forte.Redirects.Redirect;

namespace Forte.Redirects.Resolver
{
    public interface IRedirectRuleResolver
    {
        Task<IRedirect> ResolveRedirectRule(UrlPath oldPath);
    }

}