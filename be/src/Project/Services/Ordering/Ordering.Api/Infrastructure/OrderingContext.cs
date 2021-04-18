using Microsoft.EntityFrameworkCore;
using Ordering.Api.Infrastructure.Orders.Entities;

namespace Ordering.Api.Infrastructure
{
    public class OrderingContext : DbContext
    {
        public OrderingContext(DbContextOptions<OrderingContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderComment> OrderComments { get; set; }
        public DbSet<OrderState> OrderStates { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderStatusTranslation> OrderStatusTranslations { get; set; }
    }
}
