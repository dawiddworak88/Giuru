using System;

namespace Buyer.Web.Shared.Configurations
{
    public class AppSettings
    {
        public string CatalogUrl { get; set; }
        public string MediaUrl { get; set; }
        public string IdentityUrl { get; set; }
        public bool IsMarketplace { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
