namespace Forte.RedirectMiddleware.Model.RedirectType {
    public enum RedirectType {Permanent, Temporary}

    public interface IResponseStatusCodeResolver
    {
        int GetHttpResponseStatusCode(RedirectRule redirectRule);
    }

    public class Http_1_0_ResponseStatusCodeResolver : IResponseStatusCodeResolver
    {
        public int GetHttpResponseStatusCode(RedirectRule redirectRule)
        {
            switch (redirectRule.RedirectType)
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
    
    public class Http_1_1_ResponseStatusCodeResolver : IResponseStatusCodeResolver
    {
        public int GetHttpResponseStatusCode(RedirectRule redirectRule)
        {
            switch (redirectRule.RedirectType)
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