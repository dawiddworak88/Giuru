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

        public DbSet<SalesFactItem> SalesFacts { get; set; }
        public DbSet<TimeDimensionItem> TimeDimensions { get; set; }
        public DbSet<ProductDimensionItem> ProductDimensions { get; set; }
        public DbSet<ClientDimensionItem> ClientDimensions { get; set; }
        public DbSet<ProductTranslationDimensionItem> ProductTranslationDimensions { get; set; }
    }
}
