using System;
using System.Collections.Generic;

namespace Seller.Web.Shared.ApiRequestModels
{
    public class PriceRequestModel
    {
        public IEnumerable<PriceDriverRequestModel> PriceDrivers { get; set; }
        public string CurrencyThreeLetterCode { get; set; }
        public DateTime PricingDate { get; set; }
    }
}
