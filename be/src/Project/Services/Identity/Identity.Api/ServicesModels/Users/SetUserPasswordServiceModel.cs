using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.Users
{
    public class SetUserPasswordServiceModel : BaseServiceModel
    {
        public Guid? ExpirationId { get; set; }
        public string Password { get; set; }
    }
}
