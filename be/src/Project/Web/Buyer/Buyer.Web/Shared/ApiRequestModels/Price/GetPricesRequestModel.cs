using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ApiRequestModels.Price
{
    public class GetPricesRequestModel
    {
        public Guid? EnvironmentId { get; set; }
        public IEnumerable<PriceRequestModel> PriceRequests { get; set; }
    }
}
