using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Orders.Entities
{
    public class OrderItemStatusChangeCommentTranslation : EntityTranslation 
    {
        [Required]
        public string OrderItemStatusChangeComment { get; set; }
        
        [Required]
        public Guid OrderItemStatusChangeId { get; set; }
    }
}
