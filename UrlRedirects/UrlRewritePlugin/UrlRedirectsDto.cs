using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Forte.UrlRedirects.UrlRewritePlugin
{
    public enum UrlRedirectsType
    {
        System,
        Manual,
        ManualWildcard
    }

    public enum RedirectStatusCode
    {
        Permanent = 301,
        Temporary = 302
    }

    public class UrlRedirectsDto
    {
        public Guid Id { get; set; }

        public string OldUrl { get; set; }

        public string NewUrl { get; set; }

        public int ContentId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UrlRedirectsType Type { get; set; }

        public int Priority { get; set; }

        public RedirectStatusCode RedirectStatusCode { get; set; }
    }
}