using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Database.Areas.Clients.Entities
{
    public class AddressClient
    {
        [Required]
        public Guid AddressId { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
