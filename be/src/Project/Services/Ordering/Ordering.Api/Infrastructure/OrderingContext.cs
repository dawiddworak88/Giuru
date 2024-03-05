using Microsoft.EntityFrameworkCore;
using Ordering.Api.Infrastructure.Attributes.Entities;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderItemStatusChange>(entity =>
            {
                entity.HasIndex(e => new { e.IsActive, e.OrderItemId })
                .IncludeProperties(e => new { e.CreatedDate, e.LastModifiedDate, e.OrderItemStateId, e.OrderItemStatusId, e.RowVersion });
            });
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderComment> OrderComments { get; set; }
        public DbSet<OrderState> OrderStates { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderStatusTranslation> OrderStatusTranslations { get; set; }
        public DbSet<OrderAttachment> OrderAttachments { get; set; }
        public DbSet<OrderItemStatusChange> OrderItemStatusChanges { get; set; }
        public DbSet<OrderItemStatusChangeCommentTranslation> OrderItemStatusChangesCommentTranslations { get; set; }
        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<AttributeTranslation> AttributeTranslations { get; set; }
        public DbSet<AttributeOption> AttributeOptions { get; set; }
        public DbSet<AttributeOptionTranslation> AttributeOptionTranslations { get; set; }
        public DbSet<AttributeOptionSet> AttributeOptionSets { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<AttributeValueTranslation> AttributeValueTranslations { get; set; }
    }
}
