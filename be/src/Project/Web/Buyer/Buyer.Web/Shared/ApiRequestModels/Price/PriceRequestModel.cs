using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ApiRequestModels.Price
{
    public class PriceRequestModel
    {
        public IEnumerable<PriceDriverRequestModel> PriceDrivers { get; set; }
        public string CurrencyThreeLetterCode { get; set; }
        public DateTime PricingDate { get; set; }
    }
}
