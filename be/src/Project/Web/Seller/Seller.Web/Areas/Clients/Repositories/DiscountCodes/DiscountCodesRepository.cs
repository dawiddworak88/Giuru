using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Foundation.Pricing.DiscountCodes;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.ApiResponseModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Threading;
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

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, string code, string description, bool isDisabled)
        {
            var apiRequest = new ApiRequest<DiscountCodeRequestModel>
            {
                Language = language,
                Data = new DiscountCodeRequestModel
                {
                    Id = id,
                    Code = code,
                    Description = description,
                    IsDisabled = isDisabled
                },
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.DiscountCodesApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<DiscountCodeRequestModel>, DiscountCodeRequestModel, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.Data?.Id is null)
            {
                throw new CustomException(response.Message, (int)System.Net.HttpStatusCode.InternalServerError);
            }

            return response.Data.Id.Value;
        }

        public async Task<PagedResults<IEnumerable<DiscountCode>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = new PagedRequestModelBase
                {
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy
                },
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.DiscountCodesApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<DiscountCode>>>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.Data?.Data is null)
            {
                return default;
            }

            return new PagedResults<IEnumerable<DiscountCode>>(response.Data.Total, response.Data.PageSize)
            {
                Data = response.Data.Data
            };
        }

        public async Task<IEnumerable<DiscountCode>> GetAsync(string token, string language)
        {
            var discountCodes = new List<DiscountCode>();
            var pageIndex = PaginationConstants.DefaultPageIndex;

            while (true)
            {
                // A failure throws, so an unavailable list can never be mistaken for "no discount codes".
                var page = await GetAsync(token, language, null, pageIndex, PaginationConstants.DefaultPageSize, $"{nameof(DiscountCode.Code)} asc");

                if (page?.Data is null)
                {
                    throw new CustomException(string.Empty, (int)System.Net.HttpStatusCode.InternalServerError);
                }

                discountCodes.AddRange(page.Data);

                if (pageIndex >= page.PageCount || page.PageSize <= 0)
                {
                    return discountCodes;
                }

                pageIndex++;
            }
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

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return response.Data;
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

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }

        public async Task<DiscountCodeValidation> ValidateAsync(string token, string language, Guid clientId, string code, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // The API client service encodes the query values.
            var apiRequest = new ApiRequest<DiscountCodeValidationApiRequestModel>
            {
                Language = language,
                Data = new DiscountCodeValidationApiRequestModel
                {
                    Code = code,
                    ClientId = clientId
                },
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.DiscountCodesValidationApiEndpoint}"
            };

            var response = await _apiClientService
                .GetAsync<ApiRequest<DiscountCodeValidationApiRequestModel>, DiscountCodeValidationApiRequestModel, DiscountCodeValidationApiResponseModel>(apiRequest)
                .WaitAsync(cancellationToken);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            // An empty body is not an answer either.
            return response.Data is null
                ? DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable)
                : DiscountCodeValidation.FromApiResponse(response.Data.Status, response.Data.DiscountCode);
        }
    }
}
