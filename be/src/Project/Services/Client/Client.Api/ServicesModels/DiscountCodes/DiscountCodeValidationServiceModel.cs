namespace Client.Api.ServicesModels.DiscountCodes
{
    public class DiscountCodeValidationServiceModel
    {
        public DiscountCodeValidationStatus Status { get; set; }

        /// <summary>The stored spelling of the code. Set only when the status is Valid.</summary>
        public string DiscountCode { get; set; }

        public bool IsValid => Status == DiscountCodeValidationStatus.Valid;
    }
}
