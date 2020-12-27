using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using Foundation.ApiExtensions.Models.Request;
using Foundation.Extensions.Exceptions;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.ApiExtensions.Models.Response;
using System.Linq;

namespace Seller.Web.Areas.Clients.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public ClientsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task DeleteAsync(string token, string language, Guid? id)
        {
            var deleteRequestModel = new RequestModelBase
            {
                Language = language
            };

            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = deleteRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.ClientsApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode && response?.Data != null)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync(string token, string language)
        {
            var categoriesRequestModel = new PagedRequestModelBase
            {
                Language = language,
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Data = categoriesRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Identity.ClientsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<Client>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var categories = new List<Client>();

                categories.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<Client>>>(apiRequest);

                    if (!nextPagesResponse.IsSuccessStatusCode)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Count() > 0)
                    {
                        categories.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return categories;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<Client> GetClientAsync(string token, string language, Guid? id)
        {
            var request = new RequestModelBase
            {
                Language = language
            };

            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = request,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.ClientsApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Client>(apiRequest);

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

        public async Task<PagedResults<IEnumerable<Client>>> GetClientsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var clientsRequestModel = new PagedRequestModelBase
            {
                Language = language,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Data = clientsRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.ClientsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<Client>>> (apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<Client>>(response.Data.Total, response.Data.PageSize)
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

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, string name, string email, string communicationLanguage)
        {
            var requestModel = new SaveClientRequestModel
            {
                Id = id,
                Language = language,
                Name = name,
                Email = email,
                CommunicationLanguage = communicationLanguage
            };

            var apiRequest = new ApiRequest<SaveClientRequestModel>
            {
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.ClientsApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<SaveClientRequestModel>, SaveClientRequestModel, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Id != null)
            {
                return response.Data.Id.Value;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
