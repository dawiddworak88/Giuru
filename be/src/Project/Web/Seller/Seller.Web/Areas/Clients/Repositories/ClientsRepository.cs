using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.ApiResponseModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Clients.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;

        public ClientsRepository(IApiClientService apiClientService,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration)
        {
            this.apiClientService = apiClientService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
        }

        public Task DeleteAsync(string token, string language, Guid? id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync(string token, string language)
        {
            var initialPagedClients = await this.GetPagedClientsAsync(token, language, null, 1, 100);

            var clients = new List<Client>();
            clients.AddRange(initialPagedClients.Data);

            for (int i = 1; i < initialPagedClients.PageCount; i++)
            { 
                var pagedClients = await this.GetPagedClientsAsync(token, language, null, i, 100);
                clients.AddRange(pagedClients.Data);
            }

            return clients;
        }

        public async Task<Client> GetClientAsync(string token, string language, Guid? id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResults<IEnumerable<Client>>> GetClientsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResults<IEnumerable<Client>>> GetPagedClientsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage)
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
                Data = this.apiClientService.InitializeRequestModelContext(clientsRequestModel),
                AccessToken = token,
                EndpointAddress = this.servicesEndpointsConfiguration.Api.Host + this.servicesEndpointsConfiguration.Api.Endpoints.Clients
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, ClientsResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.PagedClients?.Data != null)
            {
                var clients = new List<Client>();

                foreach (var clientResponse in response.Data.PagedClients.Data)
                {
                    var client = new Client
                    {
                        Id = clientResponse.Id.Value,
                        Name = clientResponse.Name
                    };

                    clients.Add(client);
                }

                return new PagedResults<IEnumerable<Client>>(response.Data.PagedClients.Total, response.Data.PagedClients.PageSize)
                {
                    Data = clients
                };
            }

            return default;
        }

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, string name, string email, string communicationLanguage)
        {
            throw new NotImplementedException();
        }
    }
}
