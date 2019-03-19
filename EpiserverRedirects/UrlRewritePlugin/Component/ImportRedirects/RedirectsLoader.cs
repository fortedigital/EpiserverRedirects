using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CsvHelper;

namespace Forte.EpiserverRedirects.UrlRewritePlugin.Component.ImportRedirects
{
    public class RedirectsLoader
    {
        public IReadOnlyCollection<RedirectDefinition> Load(HttpPostedFileBase redirectsFile)
        {
            using (var streamReader = new StreamReader(redirectsFile.InputStream))
            using (var csv = new CsvReader(streamReader))
            {
                csv.Configuration.HasHeaderRecord = false;
                return csv.GetRecords<RedirectDefinition>().ToList();
            }
        }
    }
}