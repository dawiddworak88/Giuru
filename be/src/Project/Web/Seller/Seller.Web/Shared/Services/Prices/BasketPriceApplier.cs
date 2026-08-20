using Seller.Web.Shared.DomainModels.Prices;
using System.Collections.Generic;
using System.Linq;

namespace Seller.Web.Shared.Services.Prices
{
    public static class BasketPriceApplier
    {
        public static void ClearUntrustedPrices<T>(IList<T> basketItems) where T : IPriceableBasketLine
        {
            if (basketItems is null) return;

            foreach (var basketItem in basketItems)
            {
                basketItem.UnitPrice = null;
                basketItem.Price = null;
                basketItem.Currency = null;
            }
        }

        /// <summary>
        /// Applies price results that have already been aligned one-for-one with <paramref name="basketItems"/>.
        /// Call <see cref="AlignPrices"/> before this method when handling a raw pricing response. A <c>null</c>
        /// result represents a response that could not be aligned safely and clears all untrusted prices.
        /// </summary>
        public static bool ApplyPrices<T>(IList<T> basketItems, IList<PriceLookupResult> priceResults) where T : IPriceableBasketLine
        {
            if (basketItems is null) return true;

            if (priceResults is null)
            {
                ClearUntrustedPrices(basketItems);
                return true;
            }

            if (priceResults.Any(x => x is not null && !IsKnownStatus(x.Status))) return false;

            for (var index = 0; index < basketItems.Count; index++)
            {
                var basketItem = basketItems[index];
                var priceResult = priceResults[index];

                if (priceResult is null || priceResult.Status != PriceLookupStatus.Priced || priceResult.Price is null || string.IsNullOrWhiteSpace(priceResult.Price.CurrencyCode))
                {
                    basketItem.UnitPrice = null;
                    basketItem.Price = null;
                    basketItem.Currency = null;
                    continue;
                }

                var totalQuantity = basketItem.Quantity + basketItem.StockQuantity + basketItem.OutletQuantity;
                basketItem.UnitPrice = priceResult.Price.CurrentPrice;
                basketItem.Price = priceResult.Price.CurrentPrice * (decimal)totalQuantity;
                basketItem.Currency = priceResult.Price.CurrencyCode;
            }

            return true;
        }

        public static IList<PriceLookupResult> AlignPrices(int basketItemCount, IList<int> pricedLineIndexes, IList<PriceLookupResult> prices)
        {
            if (pricedLineIndexes is null || prices is null || prices.Count != pricedLineIndexes.Count) return null;

            var alignedPrices = new PriceLookupResult[basketItemCount];
            for (var i = 0; i < pricedLineIndexes.Count; i++)
            {
                var lineIndex = pricedLineIndexes[i];
                if (lineIndex < 0 || lineIndex >= basketItemCount) return null;
                alignedPrices[lineIndex] = prices[i];
            }
            return alignedPrices;
        }

        private static bool IsKnownStatus(PriceLookupStatus status) => status switch
        {
            PriceLookupStatus.Priced => true,
            PriceLookupStatus.AuthoritativeNoPrice => true,
            PriceLookupStatus.InvalidPriceDrivers => true,
            PriceLookupStatus.ServiceUnavailable => true,
            PriceLookupStatus.MissingResponse => true,
            PriceLookupStatus.PricesHidden => true,
            _ => false
        };
    }
}