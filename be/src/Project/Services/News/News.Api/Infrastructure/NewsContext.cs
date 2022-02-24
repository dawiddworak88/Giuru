using Microsoft.EntityFrameworkCore;
using News.Api.Infrastructure.Entities;

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

        public DbSet<Categories> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
    }
}
