using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.ErrorHandling;
using Foundation.ApiExtensions.Models.Response;

namespace Foundation.ApiExtensions.Services.ApiResponseServices
{
    public interface IApiResponseService
    {
        ApiResponse<T> EnrichResponseMessage<T>(ApiResponse<T> response, string successMessage) where T : BaseResponseModel;
        ApiResponse<BaseResponseModel> GenerateErrorApiResponse(Error error);
    }
}
