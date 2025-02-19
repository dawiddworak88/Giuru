﻿using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Clients.Entities
{
    public class Client : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Language { get; set; }

        public string PhoneNumber { get; set; }

        public Guid? CountryId { get; set; }

        public Guid? CurrencyId { get; set; }

        [Required]
        public Guid OrganisationId { get; set; }

        [Required]
        public bool IsOrganisationInformationOutdated { get; set; }

        public bool IsDisabled { get; set; }

        [Required]
        public Guid SellerId { get; set; }

        public Guid? DefaultBillingAddressId { get; set; }

        public Guid? DefaultDeliveryAddressId { get; set; }
    }
}
