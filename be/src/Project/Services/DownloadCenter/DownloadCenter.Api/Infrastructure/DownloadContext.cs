using DownloadCenter.Api.Infrastructure.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace DownloadCenter.Api.Infrastructure
{
    public class DownloadContext : DbContext
    {
        public DownloadContext(DbContextOptions<DownloadContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Entities.DownloadCenter.DownloadCenter> DownloadCenter { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<CategoryFile> CategoryFiles { get; set; }
    }
}
