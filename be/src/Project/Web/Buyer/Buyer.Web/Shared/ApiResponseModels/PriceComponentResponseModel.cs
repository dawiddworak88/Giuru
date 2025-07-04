using System;

namespace Buyer.Web.Shared.ApiResponseModels
{
    public class PriceComponentResponseModel
    {
        public Guid pricingPolicyId { get; set; }
        public Guid? PriceAdjustmentPolicyId { get; set; }
        public string PriceAdjustmentPolicyName { get; set; }
        public PriceAmountResponseModel Amount { get; set; }
        public Guid? PriceAdjustmentPolicyActionId { get; set; }
        public string PriceAdjustmentPolicyActionName { get; set; }
        public decimal PriceAdjustmentPolicyValue { get; set; }
    }
}
