using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.DiscountCodes.Entities
{
    public class DiscountCodeClient : Entity
    {
        [Required]
        public Guid DiscountCodeId { get; set; }

        [Required]
        public Guid ClientId { get; set; }
    }
}
