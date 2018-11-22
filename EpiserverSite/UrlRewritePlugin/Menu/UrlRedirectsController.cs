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
    public class UrlRedirectsController: Controller
    {
        private readonly IBootstrapper _bootstrapper;

        public UrlRedirectsController(IBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        public ActionResult Index()
        {
            var viewModel = _bootstrapper.CreateViewModel("UrlRedirects", ControllerContext, "Shell");
            viewModel.GlobalNavigationMenuType = GlobalNavigationMenuType.PullDown;

            return View("~/UrlRewritePlugin/Menu/UrlRedirects.cshtml", viewModel);
        }
    }
}