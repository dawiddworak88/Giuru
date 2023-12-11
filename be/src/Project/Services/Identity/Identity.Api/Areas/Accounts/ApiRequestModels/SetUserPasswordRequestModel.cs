using System;
using System.Collections.Generic;

namespace Identity.Api.Areas.Accounts.ApiRequestModels
{
    public class SetUserPasswordRequestModel
    {
        public Guid? Id { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public IEnumerable<string> MarketingApprovals { get; set; }
    }
}
