namespace Buyer.Web.Shared.Services.Prices
{
    public sealed class DiscountCodeCheck
    {
        private DiscountCodeCheck(string discountCode, bool isValid, bool isNotValid)
        {
            DiscountCode = discountCode;
            IsValid = isValid;
            IsNotValid = isNotValid;
        }

        /// <summary>
        /// The code's stored spelling when it is valid, otherwise the code as given. Null when there is
        /// no code, Grula is not configured or no Client is signed in.
        /// </summary>
        public string DiscountCode { get; }

        public bool IsValid { get; }

        /// <summary>True only when Client.Api answered that the code is not valid - never when it could not be asked.</summary>
        public bool IsNotValid { get; }

        public static DiscountCodeCheck None()
        {
            return new DiscountCodeCheck(null, false, false);
        }

        public static DiscountCodeCheck Valid(string validDiscountCode)
        {
            return new DiscountCodeCheck(validDiscountCode, true, false);
        }

        public static DiscountCodeCheck NotValid(string discountCode)
        {
            return new DiscountCodeCheck(discountCode, false, true);
        }

        public static DiscountCodeCheck Unchecked(string discountCode)
        {
            return new DiscountCodeCheck(discountCode, false, false);
        }
    }
}
