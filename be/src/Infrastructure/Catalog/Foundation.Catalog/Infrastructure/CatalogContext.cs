using Foundation.Catalog.Infrastructure.Categories.Entites;
using Foundation.Catalog.Infrastructure.Categories.Entities;
using Foundation.Catalog.Infrastructure.ProductAttributes.Entities;
using Foundation.Catalog.Infrastructure.Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace Foundation.Catalog.Infrastructure
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
        public DbSet<CategorySchema> CategorySchemas { get; set; }
        public DbSet<CategoryImage> CategoryImages { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeTranslation> ProductAttributeTranslations { get; set; }
        public DbSet<ProductAttributeItem> ProductAttributeItems { get; set; }
        public DbSet<ProductAttributeItemTranslation> ProductAttributeItemTranslations { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVideo> ProductVideos { get; set; }
        public DbSet<ProductFile> ProductFiles { get; set; }
    }
}
