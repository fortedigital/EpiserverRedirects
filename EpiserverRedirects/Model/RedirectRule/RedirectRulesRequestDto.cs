using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Forte.EpiserverRedirects.Model.RedirectRule;

[ModelBinder(typeof(RedirectRuleDtoModelBinder))]
public class RedirectRulesRequestDto
{
    public string Operation { get; set; }
    public List<Dictionary<string, string>> RedirectRules { get; set; }
}
