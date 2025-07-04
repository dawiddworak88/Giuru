using System;

namespace Buyer.Web.Shared.ApiRequestModels.Price
{
    public class GetPriceRequestModel : PriceRequestModel
    {
        public Guid? EnvironmentId { get; set; }
    }
}
