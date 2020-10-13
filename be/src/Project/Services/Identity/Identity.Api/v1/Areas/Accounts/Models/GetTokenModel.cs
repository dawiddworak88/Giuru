using System;

namespace Identity.Api.v1.Areas.Accounts.Models
{
    public class GetTokenModel
    {
        public string Email { get; set; }
        public Guid OrganisationId { get; set; }
        public string AppSecret { get; set; }
    }
}
