using EPiServer.Shell;
using EPiServer.Shell.ViewComposition;

namespace EpiserverSite.UrlRewritePlugin
 {
     [Component(
         Title = "Url redirects",
         Categories = "cms",
         WidgetType = "alloy/UrlRedirectsComponent",
         Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
         PlugInAreas = PlugInArea.AssetsDefaultGroup
     )]
     public class UrlRedirectsComponent
     {
     }
 }