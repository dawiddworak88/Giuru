namespace Buyer.Web.Shared.Definitions.Middlewares
{
    public static class ClaimsEnrichmentConstants
    {
        public static readonly string PaletteLoadingClaimType = "PaletteLoading";
        public static readonly string ExtraPackingClaimType = "ExtraPacking";
        public static readonly string CountryClaimType = "Country";
        public static readonly string ZipCodeClaimType = "ZipCode";
        public static readonly string CurrencyClaimType = "Currency";

        public static readonly string ExtraPackingClientFieldName = "Extra Packing";
        public static readonly string PaletteLoadingClientFieldName = "Palette Loading";
    }
}
