using System;
using System.Linq;
using Forte.EpiserverRedirects.Model.RedirectRule;

namespace Forte.EpiserverRedirects.Repository
{
    public interface IRedirectRuleRepository
    {
        IRedirectRule GetById(Guid id);
        IQueryable<IRedirectRule> GetAll();
        IRedirectRule Add(IRedirectRule redirectRule);
        IRedirectRule Update(IRedirectRule redirectRule);
        bool Delete(Guid id);
        bool ClearAll();
    }
}
