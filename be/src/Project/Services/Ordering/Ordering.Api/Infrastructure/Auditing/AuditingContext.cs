using Microsoft.EntityFrameworkCore;
using Ordering.Api.Infrastructure.Auditing.Audits;

namespace Ordering.Api.Infrastructure.Auditing
{
    public class AuditingContext : DbContext
    {
        public AuditingContext(DbContextOptions<AuditingContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<AuditEntry> AuditEntries { get; set; }
    }
}
