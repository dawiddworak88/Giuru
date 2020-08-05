using Catalog.Api.Infrastructure.Media.Entities;
using Catalog.Api.Infrastructure.Products.Entities;
using Catalog.Api.Infrastructure.Schemas.Entities;
using Catalog.Api.Infrastructure.Taxonomies.Entities;
using Catalog.Api.Infrastructure.Translations.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Infrastructure
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<DbContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<MediaItemEntity> LinkMediaItemsEntities { get; set; }
        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryProduct> LinkCategoriesProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Schema> Schemas { get; set; }
        public DbSet<Taxonomy> Taxonomies { get; set; }
        public DbSet<Translation> Translations { get; set; }
    }
}
