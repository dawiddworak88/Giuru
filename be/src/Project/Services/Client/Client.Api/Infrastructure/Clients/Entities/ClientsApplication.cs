using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Clients.Entities
{
    public class ClientsApplication : Entity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string ContactJobTitle { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string CompanyAddress { get; set; }

        [Required]
        public string CompanyCountry { get; set; }

        [Required]
        public string CompanyCity { get; set; }

        [Required]
        public string CompanyRegion { get; set; }

        [Required]
        public string CompanyPostalCode { get; set; }
        
        [Required]
        public bool IsDeliveryAddressEqualBillingAddress { get; set; }

        [Required]
        public Guid BillingAddressId { get; set; }

        [Required]
        public Guid DeliveryAddressId { get; set; }
    }
}
