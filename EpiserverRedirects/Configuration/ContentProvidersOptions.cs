using System;
using System.Linq;
using Forte.EpiserverRedirects.Menu.ContentProviders;

namespace Forte.EpiserverRedirects.Configuration;

public class ContentProvidersOptions
{
    public ContentProviderOption[] ContentProviders { get; set; }

    public string GetContentProviderKey(Guid id)
    {
        if (id == ContentProviderConstants.AllId) return ContentProviderConstants.AllKey;
        
        var contentProviderOption = ContentProviders.FirstOrDefault(cp => cp.Id == id) 
                                    ?? GetDefaultContentProviderOption();
            
        return contentProviderOption.Key;
    }
    
    public Guid GetContentProviderId(string providerKey)
    {
        if (providerKey?.ToLower() == ContentProviderConstants.AllKey || providerKey == string.Empty) return ContentProviderConstants.AllId;
        
        var contentProviderOption = ContentProviders.FirstOrDefault(cp => cp.Key == providerKey) 
                                    ?? GetDefaultContentProviderOption();
            
        return contentProviderOption.Id;
    }
    

    private ContentProviderOption GetDefaultContentProviderOption()
    {
        return ContentProviders.First();
    }
}