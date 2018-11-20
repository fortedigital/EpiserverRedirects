using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpiserverSite.UrlRewritePlugin.Menu
{
    public class UrlRedirectsController: Controller
    {
        public ActionResult Index()
        {
            return View("~/UrlRewritePlugin/Menu/UrlRedirects.cshtml");
        }
    }
}