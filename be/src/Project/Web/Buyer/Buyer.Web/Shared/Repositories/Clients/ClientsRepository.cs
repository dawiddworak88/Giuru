using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Foundation.ApiExtensions.Models.Request;
using Foundation.Extensions.Exceptions;
using Foundation.ApiExtensions.Shared.Definitions;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.Clients;
using System;
using Foundation.GenericRepository.Paginations;
using Buyer.Web.Shared.ApiRequestModels.Clients;
using System.Collections.Generic;
using System.Linq;

namespace Buyer.Web.Shared.Repositories.Clients
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public ClientsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<Client> GetClientAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Identity.ClientBySellerApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Client>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data is not null)
            {
                return response.Data;
            }

            return default;
        }

        public async Task<List<ClientFieldValue>> GetClientFieldValuesAsync(string token, string language, Guid? id)
        {
            var requestModel = new PagedClientFieldValuesRequestModel
            {
                ClientId = id,
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize
            };

            var apiRequest = new ApiRequest<PagedClientFieldValuesRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.FieldValuesApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedClientFieldValuesRequestModel>, PagedClientFieldValuesRequestModel, PagedResults<IEnumerable<ClientFieldValue>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data is not null)
            {
                var fieldsValues = new List<ClientFieldValue>();

                fieldsValues.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await _apiClientService.GetAsync<ApiRequest<PagedClientFieldValuesRequestModel>, PagedClientFieldValuesRequestModel, PagedResults<IEnumerable<ClientFieldValue>>>(apiRequest);

                    if (nextPagesResponse.IsSuccessStatusCode is false)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data is not null && nextPagesResponse.Data.Data.Count() > 0)
                    {
                        fieldsValues.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return fieldsValues;
            }

            return default;
        }
    }
}
