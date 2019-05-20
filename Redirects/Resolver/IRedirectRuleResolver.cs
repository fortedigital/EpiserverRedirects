using System.Threading.Tasks;
using Forte.Redirects.Model;
using Forte.Redirects.Redirect;

namespace Forte.Redirects.Resolver
{
    public interface IRedirectRuleResolver
    {
        Task<IRedirect> ResolveRedirectRule(UrlPath oldPath);
    }

}