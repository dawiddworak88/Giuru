using System.Collections.Generic;

namespace Buyer.Web.Shared.ApiRequestModels.Price
{
    public class PricesRequestModel
    {
        public IEnumerable<PriceRequestModel> PriceRequests { get; set; }
    }
}
