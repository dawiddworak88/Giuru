using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Helpers;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Seller.Portal.Areas.Clients.ApiRequestModels;
using Seller.Portal.Areas.Clients.ApiResponseModels;
using Seller.Portal.Areas.Clients.DomainModels;
using Seller.Portal.Shared.Configurations;

namespace Seller.Portal.Areas.Clients.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;
        private readonly ILogger logger;

        public ClientsRepository(IApiClientService apiClientService,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            ILogger<ClientsRepository> logger)
        {
            this.apiClientService = apiClientService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
            this.logger = logger;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync(string token, string language)
        {
            try
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
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());

                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");
            }

            return new List<Client>();
        }

        public async Task<PagedResults<IEnumerable<Client>>> GetPagedClientsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var clientsRequestModel = new ClientsRequestModel
            {
                Language = language,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage
            };

            var apiRequest = new ApiRequest<ClientsRequestModel>
            {
                Data = this.apiClientService.InitializeRequestModelContext(clientsRequestModel),
                AccessToken = token,
                EndpointAddress = this.servicesEndpointsConfiguration.Api.Host + this.servicesEndpointsConfiguration.Api.Endpoints.Clients
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<ClientsRequestModel>, ClientsRequestModel, ClientsResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.PagedClients?.Data != null)
            {
                var clients = new List<Client>();

                foreach (var clientResponse in response.Data.PagedClients.Data)
                {
                    var client = new Client
                    {
                        Id = clientResponse.Id,
                        Name = clientResponse.Name
                    };

                    clients.Add(client);
                }

                return new PagedResults<IEnumerable<Client>>
                {
                    Data = clients,
                    PageCount = response.Data.PagedClients.PageCount,
                    Total = response.Data.PagedClients.Total
                };
            }

            return default;
        }
    }
}
