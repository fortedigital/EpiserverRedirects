using EPiServer.Shell;
using EPiServer.Shell.ViewComposition;

namespace EpiserverSite.UrlRewritePlugin
 {
     [Component(
         Title = "Url redirects",
         Categories = "cms",
         WidgetType = "alloy/UrlRedirectsComponent",
         PlugInAreas = PlugInArea.Assets
     )]
     public class UrlRedirectsComponent
     {
     }
 }