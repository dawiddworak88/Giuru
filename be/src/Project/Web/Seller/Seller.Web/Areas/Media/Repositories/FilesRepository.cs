using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.Configurations;
using System;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.Repositories
{
    public class FilesRepository : IFilesRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public FilesRepository(IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<Guid> SaveAsync(string token, string language, byte[] file, string filename)
        {
            var requestModel = new FileRequestModelBase
            {
                Language = language,
                File = file,
                Filename = filename
            };

            var apiRequest = new ApiRequest<FileRequestModelBase>
            {
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.MediaUrl}{ApiConstants.Media.FilesApiEndpoint}"
            };

            var response = await this.apiClientService.PostMultipartFormAsync<ApiRequest<FileRequestModelBase>, FileRequestModelBase, BaseFileResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode && response?.Data != null)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }

            return response.Data.Id;
        }
    }
}
