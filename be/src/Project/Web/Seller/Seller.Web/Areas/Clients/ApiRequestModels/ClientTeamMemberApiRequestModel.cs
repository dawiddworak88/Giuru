using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class ClientTeamMemberApiRequestModel : RequestModelBase
    {
        public Guid? OrganisationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsDisabled { get; set; }
        public string ReturnUrl { get; set; }
    }
}
