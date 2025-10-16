using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.Fields
{
    public class ClientFieldsRepository : IClientFieldsRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public ClientFieldsRepository(
            IApiClientService apiClientService,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IOptions<AppSettings> options)
        {
            _apiClientService = apiClientService;
            _options = options;
            _globalLocalizer = globalLocalizer;
        }

        public async Task DeleteAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.ClientUrl}{ApiConstants.Client.FieldsApiEndpoint}/{id}"
            };

            var response = await _apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }

        public async Task<PagedResults<IEnumerable<ClientField>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
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
                EndpointAddress = $"{_options.Value.ClientUrl}{ApiConstants.Client.FieldsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<ClientField>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                foreach (var responseItem in response.Data.Data.OrEmptyIfNull())
                {
                    responseItem.Type = MapFieldTypeToText(responseItem.Type);
                }

                return new PagedResults<IEnumerable<ClientField>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data.Data
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<ClientField> GetAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.ClientUrl}{ApiConstants.Client.FieldsApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, ClientField>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
            }

            return default;
        }

        public async Task<IEnumerable<ClientField>> GetAsync(string token, string language)
        {
            var requestModel = new PagedRequestModelBase
            {
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.ClientUrl}{ApiConstants.Client.FieldsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<ClientField>>>(apiRequest);
            
            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var fields = new List<ClientField>();

                fields.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<ClientField>>>(apiRequest);

                    if (!nextPagesResponse.IsSuccessStatusCode)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Count() > 0)
                    {
                        fields.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return fields;
            }

            return default;
        }

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, string name, string type)
        {
            var requestModel = new ClientFieldRequestModel
            {
                Id = id,
                Name = name,
                Type = type
            };

            var apiRequest = new ApiRequest<ClientFieldRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.ClientUrl}{ApiConstants.Client.FieldsApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<ClientFieldRequestModel>, ClientFieldRequestModel, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Id != null)
            {
                return response.Data.Id.Value;
            }

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        private string MapFieldTypeToText(string type)
        {
            var typeMappings = new Dictionary<string, string>
            {
                { "boolean", _globalLocalizer.GetString("Boolean") },
                { "text", _globalLocalizer.GetString("String") },
                { "select", _globalLocalizer.GetString("Array") },
                { "number", _globalLocalizer.GetString("Number") }
            };

            if (typeMappings.TryGetValue(type, out string mapping))
            {
                return mapping;
            }

            return string.Empty;
        }
    }
}
