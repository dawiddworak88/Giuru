namespace Foundation.Pricing.Baskets
{
    public sealed class BasketDiscountCodeOutcome
    {
        private BasketDiscountCodeOutcome(string discountCode, string rejectionMessage, string removedMessage)
        {
            DiscountCode = discountCode;
            RejectionMessage = rejectionMessage;
            RemovedMessage = removedMessage;
        }

        /// <summary>The code to price with and persist. Null clears any stored code.</summary>
        public string DiscountCode { get; }

        /// <summary>Set when the request must be rejected instead of resolved - null otherwise.</summary>
        public string RejectionMessage { get; }

        /// <summary>Set when a stored code stopped being valid and was dropped - null otherwise.</summary>
        public string RemovedMessage { get; }

        public bool IsRejected => RejectionMessage is not null;

        public static BasketDiscountCodeOutcome ForDiscountCode(string discountCode)
        {
            return new BasketDiscountCodeOutcome(discountCode, null, null);
        }

        public static BasketDiscountCodeOutcome Rejected(string rejectionMessage)
        {
            return new BasketDiscountCodeOutcome(null, rejectionMessage, null);
        }

        public static BasketDiscountCodeOutcome Dropped(string removedMessage)
        {
            return new BasketDiscountCodeOutcome(null, null, removedMessage);
        }
    }
}
