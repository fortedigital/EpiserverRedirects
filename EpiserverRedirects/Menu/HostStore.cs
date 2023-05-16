using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Shell.Services.Rest;
using EPiServer.Web;
using Microsoft.AspNetCore.Mvc;

namespace Forte.EpiserverRedirects.Menu;

[RestStore("HostStore")]
public class HostStore : RestControllerBase
{
    private readonly ISiteDefinitionRepository _siteDefinitionRepository;
    private IEnumerable<HostDto> _allHosts;
    private readonly IList<HostDto> _options;
    public static readonly HostDto AllDto  = new(Guid.Parse("5DAE2426-814E-496A-9099-1B53517A85C9"), "All");
    public static readonly HostDto AllHostDto = new(Guid.Parse("7F757A26-1B69-486A-B335-41472ABE724A"), "All hosts");

    public HostStore(ISiteDefinitionRepository siteDefinitionRepository)
    { 
        _siteDefinitionRepository = siteDefinitionRepository;
        _options = new List<HostDto> { AllDto, AllHostDto };
    }
    
    public IEnumerable<HostDto> AllHosts
    {
        get { return _allHosts ??= _siteDefinitionRepository.List().Select(SiteDefinitionToHostDto); }
    }

    [HttpGet]
    public ActionResult Get()
    {
        var hosts = AllHosts.Prepend(AllHostDto);
        return Rest(hosts);
    }
    [HttpGet]
    public ActionResult GetForFilter()
    {
        var hosts = Enumerable.Concat(_options, AllHosts);
        return Rest(hosts);
    }

    private static HostDto SiteDefinitionToHostDto(SiteDefinition siteDefinition)
    {
        return new HostDto(siteDefinition.Id, siteDefinition.Name);
    }
}