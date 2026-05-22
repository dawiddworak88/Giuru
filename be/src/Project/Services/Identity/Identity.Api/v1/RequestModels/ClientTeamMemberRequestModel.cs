using Foundation.ApiExtensions.Models.Request;
using System;

namespace Identity.Api.v1.RequestModels
{
    public class ClientTeamMemberRequestModel : RequestModelBase
    {
        public Guid? OrganisationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsDisabled { get; set; }
        public string ReturnUrl { get; set; }
    }
}
