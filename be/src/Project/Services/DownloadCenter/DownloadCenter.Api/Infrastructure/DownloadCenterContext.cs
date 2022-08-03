using DownloadCenter.Api.Infrastructure.Entities.DownloadCenterCategories;
using Microsoft.EntityFrameworkCore;

namespace DownloadCenter.Api.Infrastructure
{
    public class DownloadCenterContext : DbContext
    {
        public DownloadCenterContext(DbContextOptions<DownloadCenterContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<DownloadCenterCategory> DownloadCenterCategories { get; set; }
        public DbSet<DownloadCenterCategoryTranslation> DownloadCenterCategoryTranslations { get; set; }
        public DbSet<DownloadCenterCategoryFile> DownloadCenterCategoryFiles { get; set; }
    }
}
