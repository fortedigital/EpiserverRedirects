using EPiServer.Shell.Navigation;
using EPiServer.Shell.Web;
using EPiServer.Shell.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpiserverSite.UrlRewritePlugin.Menu
{

    [Authorize(Roles = "CmsEditors, CmsAdmins")]
    public class UrlRedirectsMenuController: Controller
    {
        private readonly IBootstrapper _bootstrapper;

        public UrlRedirectsMenuController(IBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        [MenuItem(MenuPaths.Global + "/urlRedirects",
           SortIndex = SortIndex.Last + 20,
           Text = "Url Redirects")]
        public ActionResult Index()
        {
            var viewModel = _bootstrapper.CreateViewModel("UrlRedirectsMenu", ControllerContext, "UrlRedirects");

            return View(_bootstrapper.BootstrapperViewName,
                viewModel);
        }
    }
}