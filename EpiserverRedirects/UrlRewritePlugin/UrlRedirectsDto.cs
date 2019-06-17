using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Forte.EpiserverRedirects.UrlRewritePlugin
{
    [ModelBinder(typeof(UrlRedirectsDtoModelBinder))]
    public class UrlRedirectsDto
    {
        /// <summary>
        ///     Meant to be used ONLY when mapping from redirect model. When adding redirects, other ctors should be used
        ///     (not providing the id)
        /// </summary>
        internal UrlRedirectsDto(Guid id, string oldUrl, string newUrl, int contentId, UrlRedirectsType type,
            int priority, RedirectStatusCode redirectStatusCode)
        {
            Id = id;
            OldUrl = oldUrl.NormalizePath();
            NewUrl = newUrl;
            ContentId = contentId;
            Type = type;
            Priority = priority;
            RedirectStatusCode = redirectStatusCode;
        }

        public UrlRedirectsDto(string oldUrl, int contentId, UrlRedirectsType type, int priority,
            RedirectStatusCode redirectStatusCode) : this(Guid.Empty, oldUrl, null, contentId, type, priority,
            redirectStatusCode)
        {
        }

        public UrlRedirectsDto(string oldUrl, string newUrl, UrlRedirectsType type, int priority,
            RedirectStatusCode redirectStatusCode) : this(Guid.Empty, oldUrl, newUrl, 0, type, priority,
            redirectStatusCode)
        {
        }

        public Guid Id { get; }
        public string OldUrl { get; }
        public string NewUrl { get; }
        public int ContentId { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UrlRedirectsType Type { get; }

        public int Priority { get; }
        public RedirectStatusCode RedirectStatusCode { get; }
    }
}