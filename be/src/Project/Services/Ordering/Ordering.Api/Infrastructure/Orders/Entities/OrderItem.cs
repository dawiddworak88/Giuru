using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Orders.Entities
{
    public class OrderItem : Entity
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public string ProductSku { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public double OutletQuantity { get; set; }

        [Required]
        public double StockQuantity { get; set; }

        [Required]
        public double Quantity { get; set; }

        public string PictureUrl { get; set; }

        public string ExternalReference { get; set; }

        public DateTime? ExpectedDeliveryFrom { get; set; }

        public DateTime? ExpectedDeliveryTo { get; set; }

        public string MoreInfo { get; set; }
    }
}
