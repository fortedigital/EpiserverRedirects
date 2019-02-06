using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Forte.EpiserverRedirects.UrlRewritePlugin
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
        public Guid Id { get; }

        public string OldUrl { get; }

        public string NewUrl { get; }

        public int ContentId { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UrlRedirectsType Type { get; }

        public int Priority { get; }

        public RedirectStatusCode RedirectStatusCode { get; }

        internal UrlRedirectsDto(Guid id, string oldUrl, string newUrl, int contentId, UrlRedirectsType type, int priority, RedirectStatusCode redirectStatusCode)
        {
            Id = id;
            OldUrl = oldUrl;
            NewUrl = newUrl;
            ContentId = contentId;
            Type = type;
            Priority = priority;
            RedirectStatusCode = redirectStatusCode;
        }

        public UrlRedirectsDto(string oldUrl, int contentId, UrlRedirectsType type, int priority, RedirectStatusCode redirectStatusCode)
        {
            OldUrl = oldUrl;
            ContentId = contentId;
            Type = type;
            Priority = priority;
            RedirectStatusCode = redirectStatusCode;
        }
        
        public UrlRedirectsDto(string oldUrl, string newUrl, UrlRedirectsType type, int priority, RedirectStatusCode redirectStatusCode)
        {
            OldUrl = oldUrl;
            NewUrl = newUrl;
            Type = type;
            Priority = priority;
            RedirectStatusCode = redirectStatusCode;
        }
    }
}