using EPiServer.Data;
using EPiServer.Data.Dynamic;
using System;

namespace UrlRedirects.UrlRewritePlugin
{
    [EPiServerDataStore(AutomaticallyRemapStore = true)]
    public class UrlRewriteModel : IDynamicData
    {
        public Identity Id { get; set; }
        
        public string OldUrl { get; set; }

        public string NewUrl { get; set; }

        public int ContentId { get; set; }

        public string Type { get; set; }

        public int Priority { get; set; }

        public int RedirectStatusCode { get; set; }
    }
}