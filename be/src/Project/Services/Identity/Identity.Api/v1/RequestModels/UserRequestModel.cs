using System;

namespace Identity.Api.v1.RequestModels
{
    public class UserRequestModel
    {
        #nullable enable
        public Guid? Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CommunicationLanguage { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool? LockoutEnabled { get; set; }
        public int? AccessFailedCount { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? ReturnUrl { get; set; }
        #nullable disable
    }
}
