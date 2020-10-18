using Catalog.Api.Infrastructure.Categories.Entites;
using Catalog.Api.Infrastructure.Categories.Entities;
using Catalog.Api.Infrastructure.Products.Entities;
using Catalog.Api.Infrastructure.Schemas.Entities;
using Catalog.Api.Infrastructure.Taxonomies.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Infrastructure
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<CategoryImage> CategoryImages { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVideo> ProductVideos { get; set; }
        public DbSet<ProductFile> ProductFiles { get; set; }
        public DbSet<Schema> Schemas { get; set; }
        public DbSet<SchemaTranslation> SchemaTranslations { get; set; }
        public DbSet<Taxonomy> Taxonomies { get; set; }
        public DbSet<TaxonomyTranslation> TaxonomyTranslations { get; set; }
        public DbSet<TaxonomyImage> TaxonomyImages { get; set; }
    }
}
