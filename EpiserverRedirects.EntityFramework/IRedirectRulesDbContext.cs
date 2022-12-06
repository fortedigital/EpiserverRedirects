using Forte.EpiserverRedirects.EntityFramework.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace Forte.EpiserverRedirects.EntityFramework
{
    public interface IRedirectRulesDbContext
    {
        DbSet<RedirectRuleEntity> RedirectRules { get; }

        int SaveChanges();

        DatabaseFacade Database { get; }
    }
}
