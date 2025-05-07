using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ApiRequestModels.Price
{
    public class PriceRequestModel
    {
        public Guid EnvironmentId {get; set; }
        public IEnumerable<PriceDriverRequestModel> PriceDrivers { get; set; }
        public string CurrencyThreeLetterCode { get; set; }
        public DateTime PricingDate { get; set; }
    }
}
