using System;

namespace Seller.Web.Shared.DomainModels.Prices
{
    public class PriceComponent
    {
        public Guid PricingPolicyId { get; set; }
        public Guid? PriceAdjustmentPolicyId { get; set; }
        public string PriceAdjustmentPolicyName { get; set; }
        public PriceAmount Amount { get; set; }
    }
}
