using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.Users
{
    public class UpdateUserServiceModel : BaseServiceModel
    {
        #nullable enable
        public Guid? Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        #nullable disable
        public int? AccessFailedCount { get; set; }
    }
}
