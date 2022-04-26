using EPiServer.Data;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Microsoft.EntityFrameworkCore;

namespace EpiserverRedirects.EntityFramework.Repository
{
    public abstract class RedirectRulesDbContext : DbContext
    {
        protected RedirectRulesDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<RedirectRule> RedirectRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RedirectRule>()
                .Property(rule => rule.Id)
                .IsRequired()
                .HasConversion(
                    identity => identity.ToString(),
                    stringRepresentation => Identity.Parse(stringRepresentation));

            modelBuilder.Entity<RedirectRule>()
                .HasIndex(rule => rule.Id)
                .IsUnique();
        }
    }
}
