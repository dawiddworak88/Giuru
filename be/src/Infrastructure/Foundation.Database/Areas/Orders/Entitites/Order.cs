using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Database.Areas.Orders.Entitites
{
    public class Order : Entity
    {
        [Required]
        public Guid ClientId { get; set; }

        public Guid? WorkflowStateId { get; set; }
    }
}
