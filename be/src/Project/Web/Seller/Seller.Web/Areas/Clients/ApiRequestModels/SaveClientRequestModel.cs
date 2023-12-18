using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class SaveClientRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
        public Guid? CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public Guid OrganisationId { get; set; }
        public bool HasAccount { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
        public IEnumerable<Guid> ClientManagerIds { get; set; }
        public IEnumerable<ClientFieldValueRequestModel> FieldsValues { get; set; }
        public Guid? DefaultDeliveryAddressId { get; set; }
        public Guid? DefaultBillingAddressId { get; set; }
    }
}
