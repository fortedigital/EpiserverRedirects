using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Microsoft.AspNetCore.Http;

namespace Forte.EpiserverRedirects.Import
{
    public class RedirectsLoader
    {
        public const string Delimiter = ";";

        public IReadOnlyCollection<RedirectRuleImportRow> Load(IFormFile redirectsFile)
        {
            using var stream = redirectsFile.OpenReadStream();
            using var streamReader = new StreamReader(stream);
            using var csv = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = Delimiter
            });

            return csv.GetRecords<RedirectRuleImportRow>().ToList();
        }
    }
}
