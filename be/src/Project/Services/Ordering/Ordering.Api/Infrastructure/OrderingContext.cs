﻿using Foundation.Extensions.ModelBuilders;
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
    }
}
