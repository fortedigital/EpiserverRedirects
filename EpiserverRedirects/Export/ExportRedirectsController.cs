using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using CsvHelper;
using CsvHelper.Configuration;
using EPiServer.Web;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Import;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forte.EpiserverRedirects.Export
{
    [Authorize(Roles = "CmsEditors, CmsAdmins")]
    public class ExportRedirectsController : Controller
    {
        private readonly IRedirectRuleRepository _redirectRuleRepository;
        private readonly IMimeTypeResolver _mimeTypeResolver;

        public ExportRedirectsController(IRedirectRuleRepository redirectRuleRepository, IMimeTypeResolver mimeTypeResolver)
        {
            _redirectRuleRepository = redirectRuleRepository;
            _mimeTypeResolver = mimeTypeResolver;
        }

        [HttpGet]
        [Route(Constants.BaseRoutePath + "/Import")]
        public ActionResult Export()
        {
            var csvTemplateFileData = CreateExportFileData();

            const string csvTemplateFileName = "ExportedRedirectRules.csv";
            var contentDispositionHeader = new ContentDisposition
            {
                FileName = csvTemplateFileName,
                Inline = false,
            };

            Response.Headers.Add("Content-Disposition", contentDispositionHeader.ToString());

            var fileMimeType = _mimeTypeResolver.GetMimeMapping(contentDispositionHeader.FileName);

            return File(csvTemplateFileData, fileMimeType);
        }

        private byte[] CreateExportFileData()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.CurrentCulture)
                       {
                            Delimiter = RedirectsLoader.Delimiter,
                            HasHeaderRecord = false,
                       }))
                {
                    var redirectRules = _redirectRuleRepository
                        .GetAll()
                        .Select(RedirectRuleExportRow.CreateFromRedirectRule);

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
