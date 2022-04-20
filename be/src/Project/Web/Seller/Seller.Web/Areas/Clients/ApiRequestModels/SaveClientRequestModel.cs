using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class SaveClientRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
        public string PhoneNumber { get; set; }
        public Guid OrganisationId { get; set; }
        public bool HasAccount { get; set; }
    }
}
