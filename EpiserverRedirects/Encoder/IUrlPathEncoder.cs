using Forte.EpiserverRedirects.Model;

namespace Forte.EpiserverRedirects.Encoder
{
    public interface IUrlPathEncoder
    {
        UrlPath Encode(UrlPath urlPath);
    }
}