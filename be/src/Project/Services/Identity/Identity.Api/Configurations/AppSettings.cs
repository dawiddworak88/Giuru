using System;

namespace Identity.Api.Configurations
{
    public class AppSettings
    {
        public string BuyerUrl { get; set; }
        public string MediaUrl { get; set; }
        public string IdentityUrl { get; set; }
        public Guid SellerClientId { get; set; }
        public string Regulations { get; set; }
        public string PrivacyPolicy { get; set; }
    }
}
