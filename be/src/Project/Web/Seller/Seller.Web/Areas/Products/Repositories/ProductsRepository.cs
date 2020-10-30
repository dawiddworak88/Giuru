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
using Seller.Web.Areas.Products.ApiRequestModels;
using Seller.Web.Areas.Products.ApiResponseModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.Configurations;

namespace Seller.Web.Areas.Products.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;
        private readonly ILogger logger;

        public ProductsRepository(IApiClientService apiClientService, 
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            ILogger<ProductsRepository> logger)
        {
            this.apiClientService = apiClientService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
            this.logger = logger;
        }

        public async Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage)
        {
            try
            {
                var productsRequestModel = new ProductsRequestModel
                {
                    Language = language,
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage
                };

                var apiRequest = new ApiRequest<ProductsRequestModel>
                {
                    Data = this.apiClientService.InitializeRequestModelContext(productsRequestModel),
                    AccessToken = token,
                    EndpointAddress = this.servicesEndpointsConfiguration.Api.Host + this.servicesEndpointsConfiguration.Api.Endpoints.Products
                };

                var response = await this.apiClientService.GetAsync<ApiRequest<ProductsRequestModel>, ProductsRequestModel, ProductsResponseModel>(apiRequest);

                if (response.IsSuccessStatusCode && response.Data?.PagedProducts?.Data != null)
                {
                    var products = new List<Product>();

                    foreach (var productResponse in response.Data.PagedProducts.Data)
                    {
                        var product = new Product
                        {
                            Id = productResponse.Id,
                            Sku = productResponse.Sku,
                            Name = productResponse.Name,
                            FormData = productResponse.FormData,
                            LastModifiedDate = productResponse.LastModifiedDate,
                            CreatedDate = productResponse.CreatedDate
                        };

                        products.Add(product);
                    }

                    return new PagedResults<IEnumerable<Product>>(response.Data.PagedProducts.Total, response.Data.PagedProducts.PageSize)
                    {
                        Data = products
                    };
                }
            }
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());

                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");
            }

            return default;
        }
    }
}
