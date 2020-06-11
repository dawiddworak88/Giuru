using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Helpers;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.TenantDatabase.Shared.Definitions.EntityTypes;
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
    public class ProductSchemaRepository : IProductSchemaRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;
        private readonly ILogger logger;

        public ProductSchemaRepository(IApiClientService apiClientService,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            ILogger<ProductsRepository> logger)
        {
            this.apiClientService = apiClientService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
            this.logger = logger;
        }

        public async Task<string> GetProductSchemaAsync(string token, string language)
        {
            try
            {
                var productSchemaRequestModel = new ProductSchemaRequestModel
                {
                    Id = EntityTypeConstants.ProductId,
                    Language = language
                };

                var apiRequest = new ApiRequest<ProductSchemaRequestModel>
                {
                    Data = this.apiClientService.InitializeRequestModelContext(productSchemaRequestModel),
                    AccessToken = token,
                    EndpointAddress = this.servicesEndpointsConfiguration.Api.Host + this.servicesEndpointsConfiguration.Api.Endpoints.Schema
                };

                var response = await this.apiClientService.GetAsync<ApiRequest<ProductSchemaRequestModel>, ProductSchemaRequestModel, ProductSchemaResponseModel>(apiRequest);

                if (response.IsSuccessStatusCode && response.Data != null)
                {
                    return response.Data.Schema;
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
