using Foundation.Extensions.ModelBuilders;
using Microsoft.EntityFrameworkCore;
using Ordering.Api.Infrastructure.Orders.Entities;
using System.ComponentModel;

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
            builder.Entity<OrderItemStatusChange>()
                .HasIndex(e => new { e.OrderItemId, e.CreatedDate })
                .HasFilter("[IsActive] = 1")
                .HasDatabaseName("IX_OISC_Active_OrderItemId_CreatedDate_desc")
                .IncludeProperties(e => new {
                e.Id,
                e.LastModifiedDate,
                e.OrderItemStateId,
                e.OrderItemStatusId,
                e.RowVersion
            });

            builder.Entity<OrderItem>()
                .HasIndex(e => e.OrderId)
                .HasFilter("[IsActive] = 1")
                .HasDatabaseName("IX_OI_Active_OrderId")
                .IncludeProperties(e => new {
                    e.Id,
                    e.LastOrderItemStatusChangeId,
                    e.ProductId,
                    e.ProductSku,
                    e.ProductName,
                    e.PictureUrl,
                    e.Quantity,
                    e.StockQuantity,
                    e.OutletQuantity,
                    e.ExternalReference,
                    e.MoreInfo,
                    e.LastModifiedDate,
                    e.CreatedDate
                });

            builder.Entity<OrderItemStatusChangeCommentTranslation>()
                .HasIndex(e => e.OrderItemStatusChangeId)
                .HasFilter("[IsActive] = 1")
                .HasDatabaseName("IX_OISCCT_Active_StatusChangeId")
                .IncludeProperties(e => new {
                    e.OrderItemStatusChangeComment,
                    e.Language
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
