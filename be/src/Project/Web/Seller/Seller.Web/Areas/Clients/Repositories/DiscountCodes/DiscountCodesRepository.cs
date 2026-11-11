using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.DiscountCodes
{
    public class DiscountCodesRepository : IDiscountCodesRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public DiscountCodesRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<PagedResults<IEnumerable<DiscountCode>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var requestModel = new PagedRequestModelBase
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.DiscountCodesApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<DiscountCode>>>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.Data?.Data is not null)
            {
                return new PagedResults<IEnumerable<DiscountCode>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data.Data
                };
            }

            return default;
        }

        public async Task<DiscountCode> GetAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.DiscountCodesApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, DiscountCode>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return response.Data;
        }

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, string code, string description, bool isDisabled, IEnumerable<Guid> clientIds)
        {
            var requestModel = new DiscountCodeRequestModel
            {
                Id = id,
                Code = code,
                Description = description,
                IsDisabled = isDisabled,
                ClientIds = clientIds
            };

            var apiRequest = new ApiRequest<DiscountCodeRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.DiscountCodesApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<DiscountCodeRequestModel>, DiscountCodeRequestModel, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return response.Data?.Id ?? default;
        }

        public async Task DeleteAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.DiscountCodesApiEndpoint}/{id}"
            };

            var response = await _apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }
    }
}
