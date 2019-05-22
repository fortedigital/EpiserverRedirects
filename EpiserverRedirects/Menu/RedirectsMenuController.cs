using System.Web.Mvc;
using EPiServer.Shell.Navigation;
using EPiServer.Shell.Web.Mvc;

namespace Forte.EpiserverRedirects.Menu
{
    [Authorize(Roles = "CmsEditors, CmsAdmins")]
    public class RedirectsMenuController: Controller
    {
        private readonly IBootstrapper _bootstrapper;

        public RedirectsMenuController(IBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        [MenuItem(MenuPaths.Global + "/redirects",
           SortIndex = SortIndex.Last + 20,
           Text = "Redirects")]
        public ActionResult Index()
        {
            var viewModel = _bootstrapper.CreateViewModel("RedirectsMenu", ControllerContext, "Forte.EpiserverRedirects");

            return View(_bootstrapper.BootstrapperViewName,
                viewModel);
        }
    }
}