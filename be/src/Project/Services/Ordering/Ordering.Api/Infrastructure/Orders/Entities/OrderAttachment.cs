using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Orders.Entities
{
    public class OrderAttachment : EntityMedia
    {
        [Required]
        public Guid OrderId { get; set; }
    }
}
