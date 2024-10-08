﻿using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Clients.Entities
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

        public string PhoneNumber { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
