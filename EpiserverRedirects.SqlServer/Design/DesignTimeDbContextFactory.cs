using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EpiserverRedirects.SqlServer.Design
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<SqlRedirectRulesDbContext>
    {
        private const string DatabaseConnectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=EpiserverRedirectsLocal;Integrated Security=True;Pooling=False";

        public SqlRedirectRulesDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SqlRedirectRulesDbContext>();

            builder.UseSqlServer(DatabaseConnectionString);

            return new SqlRedirectRulesDbContext(builder.Options);
        }
    }
}
