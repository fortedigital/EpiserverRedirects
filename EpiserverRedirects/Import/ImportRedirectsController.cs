using System;
using CsvHelper;
using Forte.EpiserverRedirects.Configuration;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [Route(Constants.BaseRoutePath + "/Import")]
        public ActionResult Import(IFormFile uploadedFile)
        {
            if (uploadedFile == null)
            {
                return CreateJsonErrorResult("No file specified");
            }

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
                var missingFieldIndex = e.Context.Reader.CurrentIndex;
                var missingFieldName = RedirectRuleImportRow.FieldNames[missingFieldIndex];
                var errorMessage =
                    $"Row: '{e.Context.Parser.RawRecord.TrimEnd('\r', '\n')}' is invalid. Field: '{missingFieldName}' at index: '{missingFieldIndex}' is missing";
                return CreateJsonErrorResult(errorMessage);
            }
            catch (MissingFieldException)
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
            return Content(json, "application/json");
        }
    }
}
