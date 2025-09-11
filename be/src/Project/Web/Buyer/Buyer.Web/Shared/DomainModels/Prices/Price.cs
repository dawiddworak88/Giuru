using System.Collections.Generic;

namespace Buyer.Web.Shared.DomainModels.Prices
{
    public class Price
    {
        public decimal CurrentPrice { get; set; }
        public string CurrencyCode { get; set; }
        public List<string> Includes { get; set; }
    }
}
