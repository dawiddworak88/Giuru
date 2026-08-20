namespace Seller.Web.Shared.Definitions
{
    public struct PriceDriversConstants
    {
        public const string ProductDriver = "Product";
        public const string ProductExtraPackingDriver = "Product Extra Packing";
        public const string SleepAreaDriver = "Sleep Area";
        public const string FabricsGroupDriver = "Fabrics Group";
        public const string ClientDriver = "Client";
        public const string ClientCountryDriver = "Client Country";
        public const string ClientExtraPackingDriver = "Client Extra Packing";
        public const string PaletteLoadingDriver = "Palette Loading";
        public const string PaletteSizeDriver = "Palette Size";
        public const string DeliveryAddressDriver = "Delivery Address";

        // Purchase intent for the quoted line - always built via IPriceProductFactory, never
        // product/stock outlet-availability. See Issues.md #4 for why an omitted or state-derived
        // value here made catalog and basket prices disagree.
        public const string OutletDriver = "Outlet";
        public const string MirrorDriver = "Mirror";
        public const string SizeDriver = "Size";
        public const string ShapeDriver = "Shape";
        public const string PointsOfLightDriver = "Points of Light";
        public const string LampshadeTypeDriver = "Lampshade Type";
        public const string LampshadeSizeDriver = "Lampshade Size";
        public const string LinearLightDriver = "Linear Light";
        public const string PrimaryColorDriver = "Primary Color";
        public const string SecondaryColorDriver = "Secondary Color";
        public const string BodyColourDriver = "Body colour";
        public const string ShelfTypeDriver = "Shelf Type";
        public const string NumberOfMirrorsDriver = "Number of mirrors";
        public const string LedDriver = "LED";
        public const string CurrencyDriver = "Currency";
        public const string ProductVariantSkuDriver = "Product Variant SKU";
        public const string DiscountCodeDriver = "Discount Code";
    }
}
