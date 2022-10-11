using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Orders.Entities
{
    public class OrderItemStatusChange : Entity
    {
        [Required]
        public Guid OrderItemId { get; set; }

        [Required]
        public Guid OrderItemStatusId { get; set; }

        [Required]
        public Guid OrderItemStateId { get; set; }

        public virtual OrderItemStatusChangeCommentTranslation OrderItemStatusChangeCommentTranslation { get; set; }
    }
}
