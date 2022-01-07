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
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<InventoryItem> Inventory { get; set; }
    }
}
