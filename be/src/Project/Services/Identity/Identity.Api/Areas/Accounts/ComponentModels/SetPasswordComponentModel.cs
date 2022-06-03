using System;

namespace Identity.Api.Areas.Accounts.ComponentModels
{
    public class SetPasswordComponentModel
    {
        public Guid? Id { get; set; }
        public string ReturnUrl { get; set; }
        public string Language { get; set; }
        public string Token { get; set; }
    }
}
