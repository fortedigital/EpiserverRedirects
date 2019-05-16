using EPiServer.ServiceLocation;

namespace Forte.Redirects.Model.RedirectType {
    public enum RedirectType {Permanent = 1, Temporary = 2}

    public interface IResponseStatusCodeResolver
    {
        int GetHttpResponseStatusCode(RedirectType redirectType);
    }

    public class Http_1_0_ResponseStatusCodeResolver : IResponseStatusCodeResolver
    {
        public int GetHttpResponseStatusCode(RedirectType redirectType)
        {
            switch (redirectType)
            {
                case RedirectType.Permanent:
                    return 301;
                case RedirectType.Temporary:
                    return 302;
                default:
                    return 301;
            }
        }
    }
    
    [ServiceConfiguration(ServiceType = typeof(IResponseStatusCodeResolver))]
    public class Http_1_1_ResponseStatusCodeResolver : IResponseStatusCodeResolver
    {
        public int GetHttpResponseStatusCode(RedirectType redirectType)
        {
            switch (redirectType)
            {
                case RedirectType.Temporary:
                    return 307;
                case RedirectType.Permanent:
                    return 308;
                default:
                    return 308;
            }
        }
    }
      
}