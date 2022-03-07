using Buyer.Web.Areas.News.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.Repositories.Categories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public CategoriesRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(string token, string language)
        {
            var productsRequestModel = new PagedRequestModelBase
            {
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = productsRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.NewsUrl}{ApiConstants.News.CategoriesApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<Category>>>(apiRequest);
            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var categories = new List<Category>();

                categories.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<Category>>>(apiRequest);

                    if (!nextPagesResponse.IsSuccessStatusCode)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Any())
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
    }
}
