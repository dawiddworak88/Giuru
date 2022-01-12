using Basket.Api.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Basket.Api.Infrastructure
{
    public class BasketContext : DbContext
    {
        public BasketContext(DbContextOptions<BasketContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<BasketItems> Baskets { get; set; }
    }
}
