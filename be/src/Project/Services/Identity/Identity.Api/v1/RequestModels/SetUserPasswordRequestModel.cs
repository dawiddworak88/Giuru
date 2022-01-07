using Foundation.ApiExtensions.Models.Request;
using System;

namespace Identity.Api.v1.RequestModels
{
    public class SetUserPasswordRequestModel
    {
        public Guid? ExpirationId { get; set; }
        public string Password { get; set; }
    }
}
