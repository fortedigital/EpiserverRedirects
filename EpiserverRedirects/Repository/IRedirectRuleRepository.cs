using Forte.EpiserverRedirects.Model.RedirectRule;
using System;
using System.Collections.Generic;


namespace Forte.EpiserverRedirects.Repository
{
    /// <summary>
    /// TODO - add async support to Find*Match() methods
    /// </summary>
    public interface IRedirectRuleRepository
    {
        RedirectRuleModel GetById(Guid id);

        IList<RedirectRuleModel> GetAll();

        IList<RedirectRuleModel> Query(out int total, RedirectRuleQuery query);

        IList<RedirectRuleModel> GetByContent(IList<int> contentIds);

        RedirectRuleModel FindRegexMatch(string patern);

        RedirectRuleModel FindExactMatch(string patern);

        RedirectRuleModel Add(RedirectRuleModel redirectRule);

        RedirectRuleModel Update(RedirectRuleModel redirectRule);

        bool Delete(Guid id);

        bool ClearAll();
    }
}
