using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Infrastructure.Clients.Entities
{
    public class Client : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Language { get; set; }

        /// <summary>
        /// Organisation Id
        /// </summary>
        public Guid SellerId { get; set; }
    }
}
