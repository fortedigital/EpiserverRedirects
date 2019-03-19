using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Forte.EpiserverRedirects.UrlRewritePlugin.Component.ImportRedirects;

namespace Forte.EpiserverRedirects.UrlRewritePlugin.Component
{
    public class ImportRedirectsController : Controller
    {
        private readonly RedirectsLoader _redirectDefinitionsLoader;
        private readonly RedirectsImporter _redirectsImporter;

        public ImportRedirectsController(RedirectsLoader redirectDefinitionsLoader, RedirectsImporter redirectsImporter)
        {
            _redirectDefinitionsLoader = redirectDefinitionsLoader;
            _redirectsImporter = redirectsImporter;
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No file specified");
            }
            
            try
            {
                var redirectDefinitions = _redirectDefinitionsLoader.Load(uploadedFile);

                _redirectsImporter.ImportRedirects(redirectDefinitions);
                return Json(new
                {
                    TimeStamp = DateTime.Now.ToString("O"),
                    ImportedCount = redirectDefinitions.Count
                });
            }
            catch (Exception e) when (e is MissingFieldException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "File is in invalid format");
            }
        }
    }
}