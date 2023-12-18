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
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.FieldValues
{
    public class ClientFieldValuesRepository : IClientFieldValuesRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;

        public ClientFieldValuesRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> options)
        {
            _apiClientService = apiClientService;
            _options = options;
        }

        public async Task<IEnumerable<ClientFieldValue>> GetAsync(string token, string language)
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
                EndpointAddress = $"{_options.Value.ClientUrl}{ApiConstants.Client.FieldValuesApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<ClientFieldValue>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var fieldsValues = new List<ClientFieldValue>();

                fieldsValues.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<ClientFieldValue>>>(apiRequest);

                    if (!nextPagesResponse.IsSuccessStatusCode)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Count() > 0)
                    {
                        fieldsValues.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return fieldsValues;
            }

            return default;
        }

        public async Task SaveAsync(string token, string language, Guid? clientId, IEnumerable<ApiClientFieldValue> fieldsValues)
        {
            var requestModel = new ClientFieldValuesRequestModel
            {
                ClientId = clientId,
                FieldsValues = fieldsValues.Select(x => new ClientFieldValueRequestModel
                {
                    FieldDefinitionId = x.FieldDefinitionId,
                    FieldValue = x.FieldValue
                })
            };

            var apiRequest = new ApiRequest<ClientFieldValuesRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.ClientUrl}{ApiConstants.Client.FieldValuesApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<ClientFieldValuesRequestModel>, ClientFieldValuesRequestModel, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }
    }
}
