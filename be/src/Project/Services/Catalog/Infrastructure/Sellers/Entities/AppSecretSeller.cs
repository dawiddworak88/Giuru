using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Sellers.Entities
{
    public class AppSecretSeller : Entity
    {
        [Required]
        public Guid AppSecretId { get; set; }

        [Required]
        public Guid SellerId { get; set; }
    }
}
