using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Request
{
    public interface IRedirectHttpResponse
    {
        void Redirect(string location, RedirectType redirectType);
    }
}
