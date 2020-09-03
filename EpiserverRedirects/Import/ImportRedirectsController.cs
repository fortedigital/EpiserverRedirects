using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using Forte.EpiserverRedirects.Model.RedirectRule;
using MissingFieldException = System.MissingFieldException;

namespace Forte.EpiserverRedirects.Import
{
    [Authorize(Roles = "CmsEditors, CmsAdmins")]
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No file specified");
            
            try
            {
                var redirectDefinitions = _redirectDefinitionsLoader.Load(uploadedFile);

                _redirectsImporter.ImportRedirects(redirectDefinitions);
                return Json(new
                {
                    TimeStamp = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc).ToString("O"),
                    ImportedCount = redirectDefinitions.Count
                });
            }
            catch (Exception e) when (e is MissingFieldException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "File is in invalid format");
            }
        }

        [HttpGet]
        public ActionResult GetTemplate()
        {
            var csvTemplateFileData = CreateCsvTemplateFileData();

            const string csvTemplateFileName = "CsvTemplate.csv";
            var contentDispositionHeader = new ContentDisposition
            {
                FileName = csvTemplateFileName,
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", contentDispositionHeader.ToString());

            var fileMimeType = MimeMapping.GetMimeMapping(contentDispositionHeader.FileName);

            return File(csvTemplateFileData, fileMimeType);
        }

        private static byte[] CreateCsvTemplateFileData()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(writer, new CsvHelper.Configuration.Configuration{CultureInfo = CultureInfo.InvariantCulture}))
                {    
                    csvWriter.WriteHeader<RedirectRuleImportRow>();
                }
                return ReadAllBytes(memoryStream);
            }
        }

        private static byte[] ReadAllBytes(Stream stream)
        {
            if (stream is MemoryStream memoryStream)
                return memoryStream.ToArray();

            using (var newMemoryStream = new MemoryStream())
            {
                stream.CopyTo(newMemoryStream);
                return newMemoryStream.ToArray();
            }
        }
    }
}
