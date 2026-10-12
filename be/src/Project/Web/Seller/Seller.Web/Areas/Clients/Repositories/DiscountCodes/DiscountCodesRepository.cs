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
using System.Linq;
using System.Net;
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

        public async Task<(Guid Id, string Version)> SaveAsync(string token, string language, Guid? id, string version, string code, string description, bool isDisabled, IEnumerable<Guid> clientIds)
        {
            var apiRequest = new ApiRequest<DiscountCodeRequestModel>
            {
                Language = language,
                Data = new DiscountCodeRequestModel
                {
                    Id = id,
                    Version = version,
                    Code = code,
                    Description = description,
                    IsDisabled = isDisabled,
                    ClientIds = clientIds ?? Enumerable.Empty<Guid>()
                },
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.DiscountCodesApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<DiscountCodeRequestModel>, DiscountCodeRequestModel, DiscountCodeSaveApiResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.Data?.Id is null)
            {
                throw new CustomException(response.Message, (int)HttpStatusCode.InternalServerError);
            }

            return (response.Data.Id.Value, response.Data.Version);
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

        public async Task DeleteAsync(string token, string language, Guid? id, string version)
        {
            var apiRequest = new ApiRequest<DiscountCodeDeleteRequestModel>
            {
                Language = language,
                Data = new DiscountCodeDeleteRequestModel { Version = version },
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.DiscountCodesApiEndpoint}/{id}"
            };

            // The API client service encodes the query values.
            var response = await _apiClientService.DeleteAsync<ApiRequest<DiscountCodeDeleteRequestModel>, DiscountCodeDeleteRequestModel, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }

        public async Task<IEnumerable<DiscountCodeClientOption>> GetClientOptionsAsync(string token, string language)
        {
            var options = new List<DiscountCodeClientOption>();
            var pageIndex = PaginationConstants.DefaultPageIndex;
            long total;

            while (true)
            {
                var page = await GetClientOptionsPageAsync(token, language, pageIndex);

                if (page?.Data is null || page.PageSize <= 0)
                {
                    throw new CustomException(string.Empty, (int)HttpStatusCode.InternalServerError);
                }

                options.AddRange(page.Data);
                total = page.Total;

                if (pageIndex >= page.PageCount)
                {
                    break;
                }

                pageIndex++;
            }

            // A list that is shorter or longer than the server says it is, or repeats a client, was changed while it
            // was being read. Saving it could silently unassign clients, so the form does not load.
            if (options.Count != total || options.Select(x => x.Id).Distinct().Count() != options.Count)
            {
                throw new CustomException(string.Empty, (int)HttpStatusCode.InternalServerError);
            }

            return options;
        }

        public async Task<DiscountCodeValidation> ValidateAsync(string token, string language, Guid clientId, string code)
        {
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
                .GetAsync<ApiRequest<DiscountCodeValidationApiRequestModel>, DiscountCodeValidationApiRequestModel, DiscountCodeValidationApiResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            // An empty body is not an answer either.
            return response.Data is null
                ? DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable)
                : DiscountCodeValidation.FromApiResponse(response.Data.Status, response.Data.DiscountCode);
        }

        private async Task<PagedResults<IEnumerable<DiscountCodeClientOption>>> GetClientOptionsPageAsync(string token, string language, int pageIndex)
        {
            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = new PagedRequestModelBase
                {
                    PageIndex = pageIndex,
                    ItemsPerPage = PaginationConstants.DefaultPageSize,
                    OrderBy = $"{nameof(DiscountCodeClientOption.Name)} asc"
                },
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.DiscountCodeClientOptionsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<DiscountCodeClientOption>>>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return response.Data;
        }
    }
}
