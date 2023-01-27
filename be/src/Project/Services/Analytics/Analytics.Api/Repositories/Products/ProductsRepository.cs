using Analytics.Api.DomainModels;
using Analytics.Api.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Analytics.Api.Repositories.Products
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;

        public ProductsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> options)
        {
            _apiClientService = apiClientService;
            _options = options;
        }

        public async Task<Product> GetAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Product>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
