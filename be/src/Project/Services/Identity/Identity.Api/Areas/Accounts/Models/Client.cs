﻿using System.Collections.Generic;
using System;

namespace Identity.Api.Areas.Accounts.Models
{
    public class Client
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? OrganisationId { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
        public IEnumerable<Guid> ClientManagerIds { get; set; }
        public Guid? DefaultDeliveryAddressId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}