using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.DomainModels.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Repositories.Clients
{
    public class ClientGroupsRepository : IClientGroupsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public ClientGroupsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<IEnumerable<ClientGroup>> GetAsync(string token, string language)
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
                EndpointAddress = $"{settings.Value.ClientUrl}{ApiConstants.Client.GroupsApiEndpoint}"
            };

            var response = await apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<ClientGroup>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var groups = new List<ClientGroup>();

                groups.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<ClientGroup>>>(apiRequest);

                    if (!nextPagesResponse.IsSuccessStatusCode)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Count() > 0)
                    {
                        groups.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return groups;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
