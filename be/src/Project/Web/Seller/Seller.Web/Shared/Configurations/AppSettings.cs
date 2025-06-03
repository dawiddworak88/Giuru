﻿using System;

namespace Seller.Web.Shared.Configurations
{
    public class AppSettings
    {
        public string CatalogUrl { get; set; }
        public string MediaUrl { get; set; }
        public string CdnUrl { get; set; }
        public string IdentityUrl { get; set; }
        public string BasketUrl { get; set; }
        public string ClientUrl { get; set; }
        public string OrderUrl { get; set; }
        public string InventoryUrl { get; set; }
        public string NewsUrl { get; set; }
        public string DownloadCenterUrl { get; set; }
        public string BuyerUrl { get; set; }
        public string SellerUrl { get; set; }
        public string GlobalUrl { get; set; }
        public string ContentUrl { get; set; }
        public string AnalyticsUrl { get; set; }
        public string GrulaUrl { get; set; }
        public string GrulaAccessToken { get; set; }
        public Guid? GrulaEnvironmentId { get; set; }
        public string PossibleExtraPackingAttributeKeys { get; set; }
        public string PossiblePriceGroupAttributeKeys { get; set; }
        public string PossibleSleepAreaWidthAttributeKeys { get; set; }
        public string PossibleSleepAreaDepthAttributeKeys { get; set; }
        public string PossiblePaletteSizeAttributeKeys { get; set; }
        public string DefaultCurrency { get; set; }
    }
}
