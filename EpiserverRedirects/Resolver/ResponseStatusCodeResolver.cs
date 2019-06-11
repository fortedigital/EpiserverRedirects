using EPiServer.ServiceLocation;

namespace Forte.EpiserverRedirects.Resolver {
    public interface IResponseStatusCodeResolver
    {
        int GetHttpResponseStatusCode(Model.RedirectRule.RedirectType redirectType);
    }

    public class Http_1_0_ResponseStatusCodeResolver : IResponseStatusCodeResolver
    {
        public int GetHttpResponseStatusCode(Model.RedirectRule.RedirectType redirectType)
        {
            switch (redirectType)
            {
                case Model.RedirectRule.RedirectType.Permanent:
                    return 301;
                case Model.RedirectRule.RedirectType.Temporary:
                    return 302;
                default:
                    return 301;
            }
        }
    }
    
    [ServiceConfiguration(ServiceType = typeof(IResponseStatusCodeResolver))]
    public class Http_1_1_ResponseStatusCodeResolver : IResponseStatusCodeResolver
    {
        public int GetHttpResponseStatusCode(Model.RedirectRule.RedirectType redirectType)
        {
            switch (redirectType)
            {
                case Model.RedirectRule.RedirectType.Permanent:
                    return 308;
                case Model.RedirectRule.RedirectType.Temporary:
                    return 307;
                default:
                    return 308;
            }
        }
    }
}
 