using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Api.Infrastructure.Ordering.Orders.Entities
{
    public class OrderItem : Entity
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        public Guid? PreviewImageMediaId { get; set; }

        public string ProductSku { get; set; }

        public string ProductName { get; set; }

        public string ProductAttributes { get; set; }

        public string ExternalReference { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        public DateTime? ExpectedDeliveryFrom { get; set; }

        public DateTime? ExpectedDeliveryTo { get; set; }

        public string MoreInfo { get; set; }
    }
}
