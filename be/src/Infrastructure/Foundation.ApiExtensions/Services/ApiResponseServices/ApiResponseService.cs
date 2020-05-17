using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.ErrorHandling;
using Foundation.ApiExtensions.Models.Response;
using Foundation.Localization;
using Microsoft.Extensions.Localization;

namespace Foundation.ApiExtensions.Services.ApiResponseServices
{
    public class ApiResponseService : IApiResponseService
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public ApiResponseService(IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
        }

        public ApiResponse<T> EnrichResponseMessage<T>(ApiResponse<T> response, string successMessage) where T: BaseResponseModel
        {
            if (response.IsSuccessStatusCode)
            {
                response.Message = successMessage;
            }

            if (!response.IsSuccessStatusCode && response.Data?.Error != null)
            {
                response.Message = this.globalLocalizer["ErrorContactAdmin"] + response.Data.Error.ErrorId + " - " + response.Data.Error.ErrorSource;
            }

            if (!response.IsSuccessStatusCode && string.IsNullOrWhiteSpace(response.Message))
            {
                response.Message = this.globalLocalizer["AnErrorOccurred"];
            }

            return response;
        }

        public ApiResponse<BaseResponseModel> GenerateErrorApiResponse(Error error)
        {
            return new ApiResponse<BaseResponseModel> { Message = this.globalLocalizer["ErrorContactAdmin"] + error.ErrorId + " - " + error.ErrorSource };
        }
    }
}
