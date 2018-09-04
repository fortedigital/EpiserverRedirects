using System;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using EpiserverSite.Models.Pages;
using EpiserverSite.Models.ViewModels;

namespace EpiserverSite.Controllers
{
    public class TestPageController : PageController<TestPage>
    {
        private readonly UrlResolver _urlResolver;

        public TestPageController(UrlResolver urlResolver)
        {
            _urlResolver = urlResolver;
        }

        public ActionResult Index(TestPage currentPage)
        {
            var model = new TestViewModel(currentPage)
            {
                Number = currentPage.Number,
                ImageUrl = ContentReference.IsNullOrEmpty(currentPage.Image) ? String.Empty : _urlResolver.GetUrl(currentPage.Image),
            };

            return View("Index", model);
        }
    }
}