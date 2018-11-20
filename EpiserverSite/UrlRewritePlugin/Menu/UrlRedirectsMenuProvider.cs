using EPiServer;
using EPiServer.Shell.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverSite.UrlRewritePlugin.Menu
{
    [MenuProvider]
    public class UrlRedirectsMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            var menuItems = new List<UrlMenuItem>();
            menuItems.Add(new UrlMenuItem("Url Redirects",
                MenuPaths.Global + "/cms" + "/cmsMenuItem",
                "/UrlRedirects")
            );

            return menuItems;
        }
    }
}