using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Helpers;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ApiRequestModels;
using Seller.Web.Areas.Products.ApiResponseModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.Configurations;

namespace Seller.Web.Areas.Products.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;
        private readonly ILogger logger;

        public ProductRepository(IApiClientService apiClientService,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            ILogger<ProductsRepository> logger)
        {
            this.apiClientService = apiClientService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
            this.logger = logger;
        }

        public async Task<Product> GetProductAsync(string token, string language, Guid? id)
        {
            try
            {
                var productRequestModel = new ProductRequestModel
                {
                    Language = language,
                    Id = id
                };

                var apiRequest = new ApiRequest<ProductRequestModel>
                {
                    Data = this.apiClientService.InitializeRequestModelContext(productRequestModel),
                    AccessToken = token,
                    EndpointAddress = this.servicesEndpointsConfiguration.Api.Host + this.servicesEndpointsConfiguration.Api.Endpoints.Product
                };

                var response = await this.apiClientService.GetAsync<ApiRequest<ProductRequestModel>, ProductRequestModel, ProductResponseModel>(apiRequest);

                if (response.IsSuccessStatusCode && response.Data != null)
                {
                    
                    var product = new Product
                    {
                        Id = response.Data.Id,
                        Sku = response.Data.Sku,
                        Name = response.Data.Name,
                        FormData = response.Data.FormData,
                        LastModifiedDate = response.Data.LastModifiedDate,
                        CreatedDate = response.Data.CreatedDate
                    };

                    return product;
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
