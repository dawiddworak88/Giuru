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

namespace Seller.Web.Areas.Media.Repositories.Files
{
    public class FilesRepository : IFilesRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public FilesRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<Guid> SaveAsync(string token, string language, byte[] file, string filename, string id)
        {
            var requestModel = new FileRequestModelBase
            {
                File = file,
                Filename = filename,
                Id = id
            };
            
            var apiRequest = new ApiRequest<FileRequestModelBase>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.MediaUrl}{ApiConstants.Media.FilesApiEndpoint}"
            };

            var response = await this.apiClientService.PostMultipartFormAsync<ApiRequest<FileRequestModelBase>, FileRequestModelBase, BaseResponseModel>(apiRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data?.Id != null)
            {
                return response.Data.Id.Value;
            }

            return default;
        }
    }
}
