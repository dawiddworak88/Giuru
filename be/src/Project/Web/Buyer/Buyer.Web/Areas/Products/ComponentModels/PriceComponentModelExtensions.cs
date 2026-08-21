using Foundation.Pricing.DomainModels;

namespace Buyer.Web.Areas.Products.ComponentModels
{
    public static class PriceComponentModelExtensions
    {
        /// <summary>
        /// The server-rendered counterpart of <c>ClaimsPriceClientResolver</c>: model builders are
        /// handed the buyer's identity on the component model rather than reading it back off the
        /// principal, so the mapping to a Grula <see cref="PriceClient"/> lives here once instead of
        /// being re-inlined in every catalog and detail builder.
        /// </summary>
        public static PriceClient ToPriceClient(this PriceComponentModel componentModel, string discountCode)
        {
            return new PriceClient
            {
                Id = componentModel?.ClientId,
                Name = componentModel?.Name,
                CurrencyCode = componentModel?.CurrencyCode,
                ExtraPacking = componentModel?.ExtraPacking,
                PaletteLoading = componentModel?.PaletteLoading,
                Country = componentModel?.Country,
                DeliveryZipCode = componentModel?.DeliveryZipCode,
                DiscountCode = discountCode
            };
        }
    }
}
