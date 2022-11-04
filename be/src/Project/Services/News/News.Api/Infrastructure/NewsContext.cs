using Microsoft.EntityFrameworkCore;
using News.Api.Infrastructure.Entities.Categories;
using News.Api.Infrastructure.Entities.News;

namespace News.Api.Infrastructure
{
    public class NewsContext : DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<NewsItemsGroup> NewsItemsGroups { get; set; }
        public DbSet<NewsItemTranslation> NewsItemTranslations { get; set; }
        public DbSet<NewsItemFile> NewsItemFiles { get; set; }
    }
}
