using System;

namespace Identity.Api.Areas.Accounts.ApiRequestModels
{
    public class SetUserPasswordRequestModel
    {
        public Guid? ExpirationId { get; set; }
        public string Password { get; set; }
    }
}
