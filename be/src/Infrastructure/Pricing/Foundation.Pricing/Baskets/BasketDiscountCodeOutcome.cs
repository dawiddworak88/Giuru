namespace Foundation.Pricing.Baskets
{
    public sealed class BasketDiscountCodeOutcome
    {
        private BasketDiscountCodeOutcome(string discountCode, string rejectionMessage)
        {
            DiscountCode = discountCode;
            RejectionMessage = rejectionMessage;
        }

        /// <summary>The code to price with and persist. Null clears any stored code.</summary>
        public string DiscountCode { get; }

        /// <summary>Set when the request must be rejected instead of resolved - null otherwise.</summary>
        public string RejectionMessage { get; }

        public bool IsRejected => RejectionMessage is not null;

        public static BasketDiscountCodeOutcome ForDiscountCode(string discountCode)
        {
            return new BasketDiscountCodeOutcome(discountCode, null);
        }

        public static BasketDiscountCodeOutcome Rejected(string rejectionMessage)
        {
            return new BasketDiscountCodeOutcome(null, rejectionMessage);
        }
    }
}
