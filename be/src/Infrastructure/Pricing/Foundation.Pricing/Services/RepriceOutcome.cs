namespace Foundation.Pricing.Services
{
    public sealed class RepriceOutcome
    {
        public RepriceOutcome(bool succeeded)
        {
            Succeeded = succeeded;
        }

        /// <summary>
        /// Whether the pricing response could be applied to every line. <c>false</c> means the
        /// response could not be aligned or applied safely and the caller must reject the request
        /// rather than persist unpriced or mismatched lines.
        /// </summary>
        public bool Succeeded { get; }
    }
}
