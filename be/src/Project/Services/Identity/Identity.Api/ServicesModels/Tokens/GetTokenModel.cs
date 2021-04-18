using System;

namespace Identity.Api.ServicesModels.Tokens
{
    public class GetTokenModel
    {
        public string Email { get; set; }
        public Guid OrganisationId { get; set; }
        public string AppSecret { get; set; }
    }
}
