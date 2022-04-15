using System;
using System.Linq;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Repository
{
    public interface IRedirectRuleRepository
    {
        RedirectRule GetById(Guid id);
        IQueryable<RedirectRule> GetAll();
        RedirectRule Add(RedirectRule redirectRule);
        RedirectRule Update(RedirectRule redirectRule);
        bool Delete(Guid id);
        bool ClearAll();
    }
}
