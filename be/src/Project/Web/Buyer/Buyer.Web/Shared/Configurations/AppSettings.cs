using System;

namespace Buyer.Web.Shared.Configurations
{
    public class AppSettings
    {
        public string CatalogUrl { get; set; }
        public string IdentityUrl { get; set; }
        public string InventoryUrl { get; set; }
        public string ClientUrl { get; set; }
        public string OrderUrl { get; set; }
        public string BasketUrl { get; set; }
        public string NewsUrl { get; set; }
        public string GlobalUrl { get; set; }
        public string ContentGraphQlUrl { get; set; }
        public string MediaUrl { get; set; }
        public string AnalyticsUrl { get; set; }
        public string DownloadCenterUrl { get; set; }
        public string CdnUrl { get; set; }
        public string GrulaUrl { get; set; }
        public string GrulaAccessToken { get; set; }
        public Guid? GrulaEnvironmentId { get; set; }
        public Guid? OrganisationId { get; set; }
        public int? MaxAllowedOrderQuantity { get; set; }
        public string ProductAttributes { get; set; }
        public string GoogleTagManagerIdentifier { get; set; }
        public string MakeComplaintUrl { get; set; }
        public string EnablePricesForClients { get; set; }
        public string PossibleExtraPackingAttributeKeys { get; set; }
        public string PossiblePriceGroupAttributeKeys { get; set; }
        public string PossibleSleepAreaWidthAttributeKeys { get; set; }
        public string PossibleSleepAreaDepthAttributeKeys { get; set; }
        public string PossibleDepthAttributeKeys { get; set; }
        public string PossibleWidthAttributeKeys { get; set; }
        public string PossibleLengthAttributeKeys { get; set; }
        public string PossiblePaletteSizeAttributeKeys { get; set; }
        public string PossiblePointsOfLightAttributeKeys { get; set; }
        public string PossibleLampshadeTypeAttributeKeys { get; set; }
        public string PossibleLampshadeSizeAttributeKeys { get; set; }
        public string PossibleLinearLightAttributeKeys { get; set; }
        public string PossibleMirrorAttributeKeys { get; set; }
        public string PossibleShapeAttributeKeys { get; set; }
        public string PossiblePrimaryColorAttributeKeys { get; set; }
        public string PossibleSecondaryColorAttributeKeys { get; set; }
        public string PossibleShelfTypeAttributeKeys { get; set; }
        public Guid? ProductColorAttributeId { get; set; }
        public string DefaultCulture { get; set; }
        public string DefaultCurrency { get; set; }
    }
}
