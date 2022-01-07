using System;

namespace Identity.Api.Areas.Accounts.Models
{
    public class SetPasswordModel
    {
        public Guid? Id { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
