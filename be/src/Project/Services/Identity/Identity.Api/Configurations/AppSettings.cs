using System;

namespace Identity.Api.Configurations
{
    public class AppSettings
    {
        public string MediaUrl { get; set; }
        public string IdentityUrl { get; set; }
        public Guid SellerClientId { get; set; }
    }
}
