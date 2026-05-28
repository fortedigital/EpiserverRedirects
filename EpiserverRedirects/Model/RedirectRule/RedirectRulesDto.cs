using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Forte.EpiserverRedirects.Model.RedirectRule;

[ModelBinder(typeof(RedirectRuleDtoModelBinder))]
public class RedirectRulesDto
{
    public RedirectRuleOperation Operation { get; set; }
    public IEnumerable<RedirectRuleDto> RedirectRules { get; set; }
}
