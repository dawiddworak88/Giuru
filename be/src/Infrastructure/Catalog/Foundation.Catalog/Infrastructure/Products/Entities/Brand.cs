using Foundation.GenericRepository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Catalog.Infrastructure.Products.Entities
{
    [Index(nameof(SellerId))]
    public class Brand : Entity
    {
        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
