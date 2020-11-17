using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Redirect;

namespace Forte.EpiserverRedirects.System
{
    public interface IRuleRedirectCache
    {
        void RemoveAll();
        
        bool TryGet(UrlPath urlPath, out IRedirect resource);
        
        void Add(UrlPath urlPath, IRedirect redirect);
    }
}