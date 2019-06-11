using System.Threading.Tasks;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Redirect;

namespace Forte.EpiserverRedirects.Resolver
{
    public interface IRedirectRuleResolver
    {
        Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath);
    }

}