namespace Buyer.Web.Shared.Services.Baskets
{
    public sealed class DiscountCodeResolution
    {
        public DiscountCodeResolution(string discountCode, bool isAppliedToEmptyBasket)
        {
            DiscountCode = discountCode;
            IsAppliedToEmptyBasket = isAppliedToEmptyBasket;
        }

        /// <summary>The code to price with and persist. Null clears any stored code.</summary>
        public string DiscountCode { get; }

        /// <summary>Whether the request tries to apply a new code to a basket with no lines.</summary>
        public bool IsAppliedToEmptyBasket { get; }
    }
}
