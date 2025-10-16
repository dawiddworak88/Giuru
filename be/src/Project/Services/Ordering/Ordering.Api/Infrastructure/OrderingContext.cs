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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Existing index for OrderItemStatusChange
            builder.Entity<OrderItemStatusChange>(entity =>
            {
                entity.HasIndex(e => new { e.IsActive, e.OrderItemId })
                .IncludeProperties(e => new { e.CreatedDate, e.LastModifiedDate, e.OrderItemStateId, e.OrderItemStatusId, e.RowVersion });
            });

            // Index for Orders to optimize filtering by IsActive, SellerId and sorting by CreatedDate
            builder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => new { e.IsActive, e.SellerId, e.CreatedDate })
                .IncludeProperties(e => new { e.ClientName, e.OrderStatusId, e.OrderStateId });
            });

            // Index for Orders to optimize search by ClientName
            builder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => new { e.IsActive, e.ClientName });
            });

            builder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.CreatedDate)
                    .HasDatabaseName("IX_Orders_IsActive1_CreatedDate")
                    .HasFilter("[IsActive] = 1");
            });

            builder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => new { e.IsActive, e.CreatedDate, e.Id })
                    .HasDatabaseName("IX_Orders_IsActive_CreatedDate_Id_Filtered")
                    .HasFilter("[IsActive] = 1");
            });

            builder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => new { e.CreatedDate, e.Id })
                    .HasDatabaseName("IX_Orders_IsActive1_CreatedDate_Id")
                    .HasFilter("[IsActive] = 1")
                    .IncludeProperties(e => new { e.ClientName, e.OrderStatusId, e.OrderStateId });
            });

            // Index for OrderItems to optimize filtering by OrderId and IsActive
            builder.Entity<OrderItem>(entity =>
            {
                entity.HasIndex(e => new { e.OrderId, e.IsActive })
                .IncludeProperties(e => new { e.ProductId, e.ExternalReference, e.LastOrderItemStatusChangeId });
            });

            builder.Entity<OrderItem>(entity =>
            {
                entity.HasIndex(e => e.OrderId)
                    .HasDatabaseName("IX_OrderItems_OrderId_Cover")
                    .IncludeProperties(e => new
                    {
                        e.CreatedDate, e.Currency, e.ExternalReference, e.IsActive, e.LastModifiedDate,
                        e.LastOrderItemStatusChangeId, e.MoreInfo, e.OutletQuantity, e.PictureUrl, e.Price,
                        e.ProductId, e.ProductName, e.ProductSku, e.Quantity, e.RowVersion, e.StockQuantity, e.UnitPrice
                    });
            });

            // Index for OrderStatusTranslations to optimize lookup by OrderStatusId, Language and IsActive
            builder.Entity<OrderStatusTranslation>(entity =>
            {
                entity.HasIndex(e => new { e.OrderStatusId, e.Language, e.IsActive })
                .IncludeProperties(e => new { e.Name });
            });

            // Index for OrderItemStatusChangesCommentTranslations to optimize lookup
            builder.Entity<OrderItemStatusChangeCommentTranslation>(entity =>
            {
                entity.HasIndex(e => new { e.OrderItemStatusChangeId, e.Language, e.IsActive })
                .IncludeProperties(e => new { e.OrderItemStatusChangeComment });
            });

            // Index for OrderAttachments to optimize filtering by OrderId and IsActive
            builder.Entity<OrderAttachment>(entity =>
            {
                entity.HasIndex(e => new { e.OrderId, e.IsActive })
                .IncludeProperties(e => new { e.MediaId });
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
    }
}
