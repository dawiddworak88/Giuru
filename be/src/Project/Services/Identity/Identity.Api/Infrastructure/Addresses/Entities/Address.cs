using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace Identity.Api.Infrastructure.Addresses.Entities
{
    public class Address : Entity
    {
        public string Company { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Region { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        public string PhonePrefix { get; set; }

        public string Phone { get; set; }

        public Point Location { get; set; }

        [Required]
        public string CountryCode { get; set; }
    }
}
