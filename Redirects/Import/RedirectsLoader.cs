using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CsvHelper;
using Forte.Redirects.Model.RedirectRule;

namespace Forte.Redirects.Import
{
    public class RedirectsLoader
    {
        public IReadOnlyCollection<RedirectRuleImportRow> Load(HttpPostedFileBase redirectsFile)
        {
            using (var streamReader = new StreamReader(redirectsFile.InputStream))
            using (var csv = new CsvReader(streamReader))
            {
                csv.Configuration.HasHeaderRecord = false;
                return csv.GetRecords<RedirectRuleImportRow>().ToList();
            }
        }
    }
}