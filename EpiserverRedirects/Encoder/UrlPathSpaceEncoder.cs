using System;
using Forte.EpiserverRedirects.Model;

namespace Forte.EpiserverRedirects.Encoder
{
    public class UrlPathSpaceEncoder : IUrlPathEncoder
    {
        public UrlPath Encode(UrlPath urlPath)
        {
            var path = urlPath.ToString();
            var encodedUri = Uri.EscapeUriString(path);
            
            return UrlPath.Parse(encodedUri);
        }
    }
}