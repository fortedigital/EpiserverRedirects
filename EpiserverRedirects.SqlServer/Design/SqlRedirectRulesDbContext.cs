using EpiserverRedirects.EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;

namespace EpiserverRedirects.SqlServer.Design
{
    public class SqlRedirectRulesDbContext : RedirectRulesDbContext
    {
        public SqlRedirectRulesDbContext(DbContextOptions<SqlRedirectRulesDbContext> options) : base(options)
        {
        }
    }
}
