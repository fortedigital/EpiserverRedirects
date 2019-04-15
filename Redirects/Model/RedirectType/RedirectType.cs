namespace Forte.RedirectMiddleware.Model.RedirectType {
    public enum RedirectType {Permanent, Temporary}

    internal interface IRedirectTypeMapper
    {
        int GetHttpResponseCode(RedirectType redirectType);
    }

    public class Http_1_0_RedirectTypeMapper : IRedirectTypeMapper
    {
        public int GetHttpResponseCode(RedirectType redirectType)
        {
            return MapToHttpResponseCode(redirectType);
        }

        public static int MapToHttpResponseCode(RedirectType redirectType)
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
    
    public class Http_1_1_RedirectTypeMapper : IRedirectTypeMapper
    {
        public int GetHttpResponseCode(RedirectType redirectType)
        {
            return MapToHttpResponseCode(redirectType);
        }
        
        public static int MapToHttpResponseCode(RedirectType redirectType)
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