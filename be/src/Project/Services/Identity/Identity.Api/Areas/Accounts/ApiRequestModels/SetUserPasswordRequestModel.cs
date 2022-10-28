using System;

namespace Identity.Api.Areas.Accounts.ApiRequestModels
{
    public class SetUserPasswordRequestModel
    {
        public Guid? Id { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
