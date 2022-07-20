using DownloadCenter.Api.Infrastructure.Entities.Categories;
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

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<CategoryFile> CategoryFiles { get; set; }
    }
}
