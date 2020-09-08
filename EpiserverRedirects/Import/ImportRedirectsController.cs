using System;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using EPiServer.Shell.Web;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MissingFieldException = System.MissingFieldException;

namespace Forte.EpiserverRedirects.Import
{
    [Authorize(Roles = "CmsEditors, CmsAdmins")]
    public class ImportRedirectsController : Controller
    {
        private readonly RedirectsLoader _redirectDefinitionsLoader;
        private readonly RedirectsImporter _redirectsImporter;
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public ImportRedirectsController(RedirectsLoader redirectDefinitionsLoader, RedirectsImporter redirectsImporter)
        {
            _redirectDefinitionsLoader = redirectDefinitionsLoader;
            _redirectsImporter = redirectsImporter;
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile == null)
                return CreateJsonErrorResult("No file specified");

            try
            {
                var redirectDefinitions = _redirectDefinitionsLoader.Load(uploadedFile);

                _redirectsImporter.ImportRedirects(redirectDefinitions);
                return CreateJsonResult(new
                {
                    TimeStamp = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc).ToString("O"),
                    ImportedCount = redirectDefinitions.Count
                });
            }
            catch (CsvHelperException e)
            {
                var missingFieldIndex = e.ReadingContext.CurrentIndex;
                var missingFieldName = RedirectRuleImportRow.FieldNames[missingFieldIndex];
                var errorMessage =
                    $"Row: '{e.ReadingContext.RawRecord.TrimEnd("\n").TrimEnd("\r")}' is invalid. Field: '{missingFieldName}' at index: '{missingFieldIndex}' is missing";
                return CreateJsonErrorResult(errorMessage);
            }
            catch (MissingFieldException e)
            {
                return CreateJsonErrorResult("File is in invalid format");
            }
        }

        private ActionResult CreateJsonErrorResult(string message)
        {
            var data = new { ErrorMessage = message };
            return CreateJsonResult(data);
        }

        private ActionResult CreateJsonResult(object data)
        {
            var json = JsonConvert.SerializeObject(data, SerializerSettings);
            return this.Content(json, "application/json");
        }
    }
}
