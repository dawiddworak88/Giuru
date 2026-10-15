namespace Foundation.Pricing.Baskets
{
    public sealed class BasketDiscountCodeOutcome
    {
        private BasketDiscountCodeOutcome(string discountCode, string rejectionMessage, string removedDiscountCode)
        {
            DiscountCode = discountCode;
            RejectionMessage = rejectionMessage;
            RemovedDiscountCode = removedDiscountCode;
        }

        /// <summary>The code to price with and persist. Null clears any stored code.</summary>
        public string DiscountCode { get; }

        /// <summary>Set when the request must be rejected instead of resolved - null otherwise.</summary>
        public string RejectionMessage { get; }

        /// <summary>
        /// The stored code that is no longer valid and was dropped - null otherwise. The save still goes ahead, priced
        /// without it, and the caller tells the user that the code was removed and the prices were updated.
        /// </summary>
        public string RemovedDiscountCode { get; }

        public bool IsRejected => RejectionMessage is not null;

        public bool IsDiscountCodeRemoved => RemovedDiscountCode is not null;

        public static BasketDiscountCodeOutcome ForDiscountCode(string discountCode)
        {
            return new BasketDiscountCodeOutcome(discountCode, null, null);
        }

        public static BasketDiscountCodeOutcome Rejected(string rejectionMessage)
        {
            return new BasketDiscountCodeOutcome(null, rejectionMessage, null);
        }

        public static BasketDiscountCodeOutcome Removed(string removedDiscountCode)
        {
            return new BasketDiscountCodeOutcome(null, null, removedDiscountCode);
        }
    }
}
