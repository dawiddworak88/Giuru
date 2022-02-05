using Foundation.Extensions.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Identity.Api.ServicesModels.Users
{
    public class UpdateUserServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public int? AccessFailedCount { get; set; }
        public string Url { get; set; }
        public string Scheme { get; set; }
        public HostString Host { get; set; }
        public string ReturnUrl { get; set; }
    }
}
