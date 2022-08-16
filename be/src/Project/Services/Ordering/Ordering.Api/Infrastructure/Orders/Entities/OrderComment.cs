using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Orders.Entities
{
    public class OrderComment : Entity
    {
        [Required]
        public string Text { get; set; }

        public string Language { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public string IpAddress { get; set; }
    }
}
