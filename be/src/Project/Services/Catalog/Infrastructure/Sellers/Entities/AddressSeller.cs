using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Sellers.Entities
{
    public class AddressSeller : Entity
    {
        [Required]
        public Guid AddressId { get; set; }

        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
