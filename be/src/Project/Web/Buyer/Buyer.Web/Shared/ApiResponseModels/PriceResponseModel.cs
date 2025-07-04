using Foundation.ApiExtensions.Models.Response;

namespace Buyer.Web.Shared.ApiResponseModels
{
    public class PriceResponseModel : BaseResponseModel
    {
        public PriceAmountResponseModel Amount { get; set; }
    }
}
