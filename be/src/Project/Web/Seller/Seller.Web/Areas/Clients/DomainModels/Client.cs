using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.DomainModels
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
        public Guid? CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
        public IEnumerable<Guid> ClientManagerIds { get; set; }
        public Guid? DefaultDeliveryAddressId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEmailMarketingApproval { get; set; }
        public bool IsSmsMarketingApproval { get; set; }
    }
}
