using EPiServer.Shell.Navigation;
using EPiServer.Shell.Web.Mvc;
using Forte.EpiserverRedirects.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Forte.EpiserverRedirects.Menu
{
    [Authorize(Roles = "CmsEditors, CmsAdmins")]
    public class RedirectsMenuController: Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IBootstrapper _bootstrapper;

        public RedirectsMenuController(IBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        [MenuItem(MenuPaths.Global + "/redirects",
           SortIndex = SortIndex.Last + 20,
           Text = "Redirects")]
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            var viewModel = _bootstrapper.CreateViewModel("RedirectsMenu", ControllerContext, Constants.ModuleName);

            return View(_bootstrapper.BootstrapperViewName,
                viewModel);
        }
    }
}