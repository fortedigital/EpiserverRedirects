using EPiServer.Shell;
using EPiServer.Shell.ViewComposition;

namespace EpiserverSite.UrlRewritePlugin
 {
     [Component(
         Title = "Url redirects",
         Categories = "cms",
         WidgetType = "urlRewritePlugin-urlRedirectsComponent/UrlRedirectsComponent",
         PlugInAreas = PlugInArea.Assets
     )]
     public class UrlRedirectsComponent
     {
     }
 }