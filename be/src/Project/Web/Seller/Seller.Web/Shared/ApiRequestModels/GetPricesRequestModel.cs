using System.Collections.Generic;
using System;

namespace Seller.Web.Shared.ApiRequestModels
{
    public class GetPricesRequestModel
    {
        public Guid? EnvironmentId { get; set; }
        public IEnumerable<PriceRequestModel> PriceRequests { get; set; }
    }
}
