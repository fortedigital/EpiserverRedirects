using Forte.EpiserverRedirects.EntityFramework.Model;
using Microsoft.EntityFrameworkCore;


namespace Forte.EpiserverRedirects.EntityFramework
{
    public interface IRedirectRulesDbContext
    {
        DbSet<RedirectRuleEntity> RedirectRules { get; }

        int SaveChanges();
    }

    public abstract class RedirectRulesDbContext : DbContext, IRedirectRulesDbContext
    {
        protected RedirectRulesDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<RedirectRuleEntity> RedirectRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RedirectRuleEntity>()
                .HasKey(rule => rule.Id);

            modelBuilder.Entity<RedirectRuleEntity>()
                .Property(rule => rule.Id)
                .IsRequired();

            modelBuilder.Entity<RedirectRuleEntity>()
                .HasIndex(rule => rule.Id)
                .IsUnique();
        }
    }
}
