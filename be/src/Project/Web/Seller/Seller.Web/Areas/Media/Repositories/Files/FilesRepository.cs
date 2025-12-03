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

        public async Task SaveChunkAsync(string token, string language, byte[] file, string filename, int? chunkNumber, string uploadId)
        {
            var requestModel = new FileRequestModelBase
            {
                UploadId = uploadId,
                File = file,
                Filename = filename,
                ChunkNumber = chunkNumber
            };

            var apiRequest = new ApiRequest<FileRequestModelBase>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.MediaUrl}{ApiConstants.Media.FileChunksApiEndpoint}"
            };

            var response = await this.apiClientService.PostMultipartFormAsync<ApiRequest<FileRequestModelBase>, FileRequestModelBase, BaseResponseModel>(apiRequest);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }
        }

        public async Task<Guid> SaveChunksCompleteAsync(string token, string language, Guid? id, string filename, Guid uploadId)
        {
            var requestModel = new FileChunksSaveCompleteRequestModel
            {
                Id = id,
                UploadId = uploadId,
                Filename = filename
            };

            var apiRequest = new ApiRequest<FileChunksSaveCompleteRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.MediaUrl}{ApiConstants.Media.FileChunksSaveCompleteApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<FileChunksSaveCompleteRequestModel>, FileChunksSaveCompleteRequestModel, BaseResponseModel>(apiRequest);

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
