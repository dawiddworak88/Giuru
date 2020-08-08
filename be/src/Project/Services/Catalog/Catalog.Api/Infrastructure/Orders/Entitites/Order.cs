using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Orders.Entitites
{
    public class Order : Entity
    {
        [Required]
        public Guid ClientId { get; set; }

        public Guid? WorkflowStateId { get; set; }
    }
}
