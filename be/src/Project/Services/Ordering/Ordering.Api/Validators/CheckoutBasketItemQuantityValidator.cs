using System;

namespace Ordering.Api.Validators
{
    internal static class CheckoutBasketItemQuantityValidator
    {
        public static bool IsValid(double quantity, double stockQuantity, double outletQuantity)
        {
            if (!IsFinite(quantity) || !IsFinite(stockQuantity) || !IsFinite(outletQuantity))
            {
                return false;
            }

            if (quantity < 0 || stockQuantity < 0 || outletQuantity < 0)
            {
                return false;
            }

            if (outletQuantity > 0 && (quantity > 0 || stockQuantity > 0))
            {
                return false;
            }

            return quantity + stockQuantity + outletQuantity > 0;
        }

        private static bool IsFinite(double value) => !double.IsNaN(value) && !double.IsInfinity(value);
    }
}