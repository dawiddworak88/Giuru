using Foundation.ApiExtensions.Models.Response;

namespace Seller.Web.Shared.ApiResponseModels
{
    public class PriceResponseModel : BaseResponseModel
    {
        public PriceAmountResponseModel Amount { get; set; }
    }
}
