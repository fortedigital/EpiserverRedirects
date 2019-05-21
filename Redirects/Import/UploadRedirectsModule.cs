using System.Web.Mvc;
using System.Web.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace Forte.Redirects.Import
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class UploadRedirectsModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            RouteTable.Routes.MapRoute(null,
                "Redirects/Import/",
                new {controller = "ImportRedirects", action = nameof(ImportRedirectsController.Import)});
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}