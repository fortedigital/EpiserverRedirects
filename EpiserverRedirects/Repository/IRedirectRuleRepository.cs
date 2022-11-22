using Forte.EpiserverRedirects.Model;
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

        SearchResult<RedirectRuleModel> Query(RedirectRuleQuery query);

        IList<RedirectRuleModel> GetByContent(IList<int> contentIds);

        RedirectRuleModel FindRegexMatch(string pattern);

        RedirectRuleModel FindExactMatch(string patern);

        RedirectRuleModel Add(RedirectRuleModel redirectRule);

        RedirectRuleModel Update(RedirectRuleModel redirectRule);

        bool Delete(Guid id);

        bool ClearAll();
    }
}
