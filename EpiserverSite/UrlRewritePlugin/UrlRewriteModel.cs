using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace EpiserverSite.UrlRewritePlugin
{
    public class UrlRewriteModel : IDynamicData
    {
        public Identity Id { get; set; }
        
        public string OldUrl { get; set; }

        public string NewUrl { get; set; }
    }
}