using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Settings.ApiRequestModels;
using Seller.Web.Areas.Settings.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Settings.Repositories
{
    public class SettingsRepository : ISettingRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public SettingsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<Dictionary<string, string>> GetAsync(string token, string language, Guid? sellerId)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.GlobalUrl}{ApiConstants.Global.SettingsApiEndpoint}/{sellerId}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Setting>(apiRequest);

            if (response.IsSuccessStatusCode)
            {
                return response.Data.Settings;
            }

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task PostAsync(string token, string language, Guid? sellerId, Dictionary<string, string> settings)
        {
            var apiRequest = new ApiRequest<SaveSettingsRequestModel>
            {
                Language = language,
                Data = new SaveSettingsRequestModel
                {
                    SellerId = sellerId,
                    Settings = settings
                },
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.GlobalUrl}{ApiConstants.Global.SettingsApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<SaveSettingsRequestModel>, SaveSettingsRequestModel, BaseResponseModel>(apiRequest);
            
            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

        }
    }
}
