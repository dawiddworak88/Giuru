using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Helpers;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.ApiRequestModels;
using Tenant.Portal.Areas.Products.ApiResponseModels;
using Tenant.Portal.Areas.Products.DomainModels;
using Tenant.Portal.Shared.Configurations;

namespace Tenant.Portal.Areas.Products.Repositories
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
                        FormData = response.Data.FormData.Replace("\r\n", string.Empty),
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
