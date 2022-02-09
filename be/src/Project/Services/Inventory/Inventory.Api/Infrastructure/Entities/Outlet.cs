using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.Infrastructure.Entities
{
    public class Outlet : Entity
    {
        [Required]
        public Guid? ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductSku { get; set; }
    }
}
