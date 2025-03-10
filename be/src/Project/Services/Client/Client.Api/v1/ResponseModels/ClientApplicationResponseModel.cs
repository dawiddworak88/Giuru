﻿using System;

namespace Client.Api.v1.ResponseModels
{
    public class ClientApplicationResponseModel
    {
        public Guid? Id { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactJobTitle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CommunicationLanguage { get; set; }
        public bool IsDeliveryAddressEqualBillingAddress { get; set; }
        public ClientApplicationAddressResponseModel BillingAddress { get; set; }
        public ClientApplicationAddressResponseModel DeliveryAddress { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
