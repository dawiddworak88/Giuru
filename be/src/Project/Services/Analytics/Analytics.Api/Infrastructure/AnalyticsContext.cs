using Analytics.Api.Infrastructure.Entities.SalesAnalytics;
using Microsoft.EntityFrameworkCore;

namespace Analytics.Api.Infrastructure
{
    public class AnalyticsContext : DbContext
    {
        public AnalyticsContext(DbContextOptions<AnalyticsContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<SalesFact> SalesFacts { get; set; }
        public DbSet<TimeDimension> TimeDimensions { get; set; }
        public DbSet<ProductDimension> ProductDimensions { get; set; }
        public DbSet<ClientDimension> ClientDimensions { get; set; }
        public DbSet<LocationDimension> LocationDimensions { get; set; }
        public DbSet<LocationTranslationDimension> LocationTranslationDimensions { get; set; }
        public DbSet<ProductTranslationDimension> ProductTranslationDimensions { get; set; }
    }
}
