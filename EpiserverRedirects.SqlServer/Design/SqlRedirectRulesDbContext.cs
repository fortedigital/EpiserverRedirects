using Forte.EpiserverRedirects.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Forte.EpiserverRedirects.SqlServer.Design
{
    public class SqlRedirectRulesDbContext : RedirectRulesDbContext
    {
        public SqlRedirectRulesDbContext(DbContextOptions<SqlRedirectRulesDbContext> options) : base(options)
        {
        }
    }
}
