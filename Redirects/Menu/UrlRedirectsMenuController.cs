using System.Web.Mvc;
using EPiServer.Shell.Navigation;
using EPiServer.Shell.Web.Mvc;

namespace Forte.Redirects.Menu
{
    [Authorize(Roles = "CmsEditors, CmsAdmins")]
    public class UrlRedirectsMenuController: System.Web.Mvc.Controller
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
            var viewModel = _bootstrapper.CreateViewModel("UrlRedirectsMenu", ControllerContext, "Redirects");

            return View(_bootstrapper.BootstrapperViewName,
                viewModel);
        }
    }
}