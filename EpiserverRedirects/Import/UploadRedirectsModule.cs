using System.Web.Mvc;
using System.Web.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Forte.EpiserverRedirects.Export;

namespace Forte.EpiserverRedirects.Import
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class UploadRedirectsModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            RouteTable.Routes.MapRoute(null,
                "Forte.EpiserverRedirects/Import/",
                new {controller = "ImportRedirects", action = nameof(ImportRedirectsController.Import)});
            RouteTable.Routes.MapRoute(null,
                "Forte.EpiserverRedirects/Export/",
                new {controller = "ExportRedirects", action = nameof(ExportRedirectsController.Export)});
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}
