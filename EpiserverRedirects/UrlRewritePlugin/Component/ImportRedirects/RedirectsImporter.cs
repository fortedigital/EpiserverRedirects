using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace Forte.EpiserverRedirects.UrlRewritePlugin.Component.ImportRedirects
{
    public class RedirectsImporter
    {
        private readonly IUrlRedirectsService _redirectsService;
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;
        private readonly IUrlResolver _urlResolver;

        public RedirectsImporter(IUrlRedirectsService redirectsService, ISiteDefinitionRepository siteDefinitionRepository, IUrlResolver urlResolver)
        {
            this._redirectsService = redirectsService;
            this._siteDefinitionRepository = siteDefinitionRepository;
            this._urlResolver = urlResolver;
        }

        public void ImportRedirects(IEnumerable<RedirectDefinition> redirectsToImport)
        {
            foreach (var redirectDefinition in redirectsToImport)
            {
                var dto = CreateDto(redirectDefinition);
                _redirectsService.Put(dto);
            }
        }

        private UrlRedirectsDto CreateDto(RedirectDefinition redirectDefinition)
        {
            var siteUrls = this._siteDefinitionRepository
                .List()
                .Select(x=>x.SiteUrl)
                .ToList();
            
            
            var contentLink = GetContentLink(siteUrls, redirectDefinition.To);
            
            return contentLink == null
                ? new UrlRedirectsDto(redirectDefinition.From, redirectDefinition.To, UrlRedirectsType.System, 1,
                    RedirectStatusCode.Permanent)
                : new UrlRedirectsDto(redirectDefinition.From, contentLink.ID, UrlRedirectsType.System, 1,
                    RedirectStatusCode.Permanent);
        }

        private ContentReference GetContentLink(List<Uri> siteUrls, string redirectRoute)
        {
            foreach (var siteUrl in siteUrls)
            {
                var content = _urlResolver.Route(new UrlBuilder($"{siteUrl}/{redirectRoute}"));
                if (content != null)
                    return content.ContentLink;
            }

            return null;
        }
    }
}