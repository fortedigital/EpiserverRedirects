using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using Forte.EpiserverRedirects.Import;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;

namespace Forte.EpiserverRedirects.Export
{
    [Authorize(Roles = "CmsEditors, CmsAdmins")]
    public class ExportRedirectsController : Controller
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;

        public ExportRedirectsController(IRedirectRuleRepository redirectRuleRepository)
        {
            _redirectRuleRepository = redirectRuleRepository;
        }

        [HttpGet]
        public ActionResult Export()
        {
            var csvTemplateFileData = CreateExportFileData();

            const string csvTemplateFileName = "ExportedRedirectRules.csv";
            var contentDispositionHeader = new ContentDisposition
            {
                FileName = csvTemplateFileName,
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", contentDispositionHeader.ToString());

            var fileMimeType = MimeMapping.GetMimeMapping(contentDispositionHeader.FileName);

            return File(csvTemplateFileData, fileMimeType);
        }

        private byte[] CreateExportFileData()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(writer,
                    new CsvHelper.Configuration.Configuration
                    {
                        CultureInfo = CultureInfo.InvariantCulture,
                        Delimiter = RedirectsLoader.Delimiter,
                        HasHeaderRecord = false,
                    }))
                {
                    var redirectRules = _redirectRuleRepository
                        .GetAll()
                        .Select(RedirectRuleImportRow.CreateFromRedirectRule);
                    
                    csvWriter.WriteRecords(redirectRules);
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
