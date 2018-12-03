using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverSite.modules.UrlRedirects.UrlRewritePlugin.Menu
{
    public class UrlRedirectsMenuViewModel
    {
        public Guid Id { get; set; }

        public string OldUrl { get; set; }

        public string NewUrl { get; set; }

        public string Type { get; set; }

        public int Priority { get; set; }
    }
}