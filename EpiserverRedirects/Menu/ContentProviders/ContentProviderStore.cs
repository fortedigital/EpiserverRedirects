using System.Collections.Generic;
using System.Linq;
using EPiServer.Shell.Services.Rest;
using Forte.EpiserverRedirects.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Forte.EpiserverRedirects.Menu.ContentProviders;

[RestStore("ContentProviderStore")]
public class ContentProviderStore : RestControllerBase
{
    private readonly ContentProvidersOptions _contentProvidersOptions;
    private static readonly ContentProviderOptionDto[] DefaultOptions = { new(ContentProviderConstants.AllId, ContentProviderConstants.AllName)};

    public ContentProviderStore(IOptions<ContentProvidersOptions> contentProvidersOptions)
    {
        _contentProvidersOptions = contentProvidersOptions.Value;
    }

    [HttpGet]
    public ActionResult GetContentProviders()
    {
        var contentProviderOptions = GetContentProviderOptions();
        
        return Rest(contentProviderOptions);
    }


    [HttpGet]
    public ActionResult GetContentProvidersForFilter()
    {
        var contentProviderOptions = GetContentProviderOptions();
        
        return Rest(DefaultOptions.Concat(contentProviderOptions));
    }

    private IEnumerable<ContentProviderOptionDto> GetContentProviderOptions()
    {
        return _contentProvidersOptions
            .ContentProviders
            .Select(x => new ContentProviderOptionDto(x.Id, x.Name));
    }

}