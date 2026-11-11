using System;

namespace Basket.Api.ServicesModelsValidators
{
    public static class BasketItemQuantityValidator
    {
        public static bool TryValidate(
            double quantity,
            double stockQuantity,
            double outletQuantity,
            int? maxAllowedOrderQuantity,
            out string errorMessage)
        {
            if (!IsFinite(quantity) || !IsFinite(stockQuantity) || !IsFinite(outletQuantity))
            {
                errorMessage = "Quantities must be finite numbers";
                return false;
            }

            if (quantity < 0)
            {
                errorMessage = "Quantity cannot be negative";
                return false;
            }

            if (stockQuantity < 0)
            {
                errorMessage = "Stock quantity cannot be negative";
                return false;
            }

            if (outletQuantity < 0)
            {
                errorMessage = "Outlet quantity cannot be negative";
                return false;
            }

            if (outletQuantity > 0 && (quantity > 0 || stockQuantity > 0))
            {
                errorMessage = "Outlet quantity cannot be combined with regular or stock quantity";
                return false;
            }

            var totalQuantity = quantity + stockQuantity + outletQuantity;
            if (totalQuantity <= 0)
            {
                errorMessage = "Total quantity must be greater than 0";
                return false;
            }

            if (maxAllowedOrderQuantity.HasValue && totalQuantity > maxAllowedOrderQuantity.Value)
            {
                errorMessage = $"Total quantity cannot exceed {maxAllowedOrderQuantity.Value}";
                return false;
            }

            errorMessage = null;
            return true;
        }

        private static bool IsFinite(double value) => !double.IsNaN(value) && !double.IsInfinity(value);
    }
}