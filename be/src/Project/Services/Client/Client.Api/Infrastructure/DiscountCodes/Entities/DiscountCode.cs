using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.DiscountCodes.Entities
{
    public class DiscountCode : Entity
    {
        [Required]
        [MaxLength(64)]
        public string Code { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public bool IsDisabled { get; set; }

        [Required]
        public Guid SellerId { get; set; }
    }
}
