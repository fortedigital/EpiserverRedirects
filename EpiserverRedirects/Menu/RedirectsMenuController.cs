using EPiServer.Shell.Navigation;
using EPiServer.Shell.Web.Mvc;
using Forte.EpiserverRedirects.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [MenuItem(MenuPaths.Global + "/redirects", SortIndex = SortIndex.Last + 20, Text = "Redirects")]
        public ActionResult Index()
        {
            var viewModel = _bootstrapper.CreateViewModel("RedirectsMenu", ControllerContext, Constants.ModuleName);
            ViewData["Layout"] = "/CmsUIViews/Views/Shared/Sleek.cshtml";

            return View(_bootstrapper.BootstrapperViewName, viewModel);
        }
    }
}