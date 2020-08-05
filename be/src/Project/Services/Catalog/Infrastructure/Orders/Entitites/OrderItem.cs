using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Orders.Entitites
{
    public class OrderItem : Entity
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public Guid OrderItemStateId { get; set; }

        [Required]
        public Guid OrderItemStatusId { get; set; }

        public Guid? SchemaId { get; set; }

        public string FormData { get; set; }
    }
}
