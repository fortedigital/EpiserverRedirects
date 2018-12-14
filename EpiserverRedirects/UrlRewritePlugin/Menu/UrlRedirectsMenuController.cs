using System.Web.Mvc;
using EPiServer.Shell.Navigation;
using EPiServer.Shell.Web.Mvc;

namespace Forte.EpiserverRedirects.UrlRewritePlugin.Menu
{
    [Authorize(Roles = "CmsEditors, CmsAdmins")]
    public class UrlRedirectsMenuController: Controller
    {
        private readonly IBootstrapper _bootstrapper;

        public UrlRedirectsMenuController(IBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        [MenuItem(MenuPaths.Global + "/redirects",
           SortIndex = SortIndex.Last + 20,
           Text = "Redirects")]
        public ActionResult Index()
        {
            var viewModel = _bootstrapper.CreateViewModel("UrlRedirectsMenu", ControllerContext, "EpiserverRedirects");

            return View(_bootstrapper.BootstrapperViewName,
                viewModel);
        }
    }
}