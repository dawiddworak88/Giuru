using Inventory.Api.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Infrastructure
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductsGroup> ProductsGroups { get; set; }
        public DbSet<InventoryItem> Inventory { get; set; }
        public DbSet<OutletItem> Outlet { get; set; }
        public DbSet<OutletItemTranslation> OutletTranslations { get; set; }
    }
}
