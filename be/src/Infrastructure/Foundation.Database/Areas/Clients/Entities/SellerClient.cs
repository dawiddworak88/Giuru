using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Database.Areas.Clients.Entities
{
    public class SellerClient : Entity
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public Guid SellerId { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
