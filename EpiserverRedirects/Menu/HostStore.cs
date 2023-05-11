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
    private  List<HostDto> _options;

    public HostStore(ISiteDefinitionRepository siteDefinitionRepository)
    { 
        _siteDefinitionRepository = siteDefinitionRepository;
    }

    public List<HostDto> Options
    {
        get {return _options ??= new List<HostDto>()
        {
            new HostDto() { Id = "1", Name = "All" }, //no filter
            new HostDto() { Id = "0", Name = "All hosts" }
        };}
    }
    public IEnumerable<HostDto> AllHosts
    {
        get { return _allHosts ??= _siteDefinitionRepository.List().Select(SiteDefinitionToHostDto); }
    }

    [HttpGet]
    public ActionResult Get()
    {
        var hosts = AllHosts.Prepend(Options[1]);
        return Rest(hosts);
    }
    [HttpGet]
    public ActionResult GetForFilter()
    {
        var hosts = Enumerable.Concat(Options, AllHosts);
        return Rest(hosts);
    }

    public HostDto SiteDefinitionToHostDto(SiteDefinition siteDefinition)
    {
        var host = new HostDto
        {
            Id = siteDefinition.Id.ToString(),
            Name = siteDefinition.Name
        };
        return host;
    }
}