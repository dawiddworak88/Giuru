namespace Client.Api.v1.ResponseModels
{
    /// <summary>
    /// The answer to "can this code be applied for this client". The status is an explicit string so callers
    /// do not share an enum with this service: Valid, NotApplicable (anything a buyer may be told), or one of
    /// the specific reasons NotFound, Disabled, NotAssigned and ClientUnknown, which only sellers receive.
    /// </summary>
    public class DiscountCodeValidationResponseModel
    {
        public string Status { get; set; }

        /// <summary>The stored spelling of the code. Set only when the status is Valid.</summary>
        public string DiscountCode { get; set; }
    }
}
