using CsvHelper.Configuration.Attributes;

namespace Forte.EpiserverRedirects.UrlRewritePlugin.Component.ImportRedirects
{
    public class RedirectDefinition
    {
        [Index(0)]
        public string From { get; set; }
        
        [Index(1)]
        public string To { get; set; }
    }
}