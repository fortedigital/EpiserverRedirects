using EPiServer.ServiceLocation;

namespace Forte.Redirects.Model {
    public interface IResponseStatusCodeResolver
    {
        int GetHttpResponseStatusCode(RedirectRule.RedirectType redirectType);
    }

    public class Http_1_0_ResponseStatusCodeResolver : IResponseStatusCodeResolver
    {
        public int GetHttpResponseStatusCode(RedirectRule.RedirectType redirectType)
        {
            switch (redirectType)
            {
                case RedirectRule.RedirectType.Permanent:
                    return 301;
                case RedirectRule.RedirectType.Temporary:
                    return 302;
                default:
                    return 301;
            }
        }
    }
    
    [ServiceConfiguration(ServiceType = typeof(IResponseStatusCodeResolver))]
    public class Http_1_1_ResponseStatusCodeResolver : IResponseStatusCodeResolver
    {
        public int GetHttpResponseStatusCode(RedirectRule.RedirectType redirectType)
        {
            switch (redirectType)
            {
                case RedirectRule.RedirectType.Temporary:
                    return 307;
                case RedirectRule.RedirectType.Permanent:
                    return 308;
                default:
                    return 308;
            }
        }
    }
      
}