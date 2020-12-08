using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ApiResponseModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.Configurations;
using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Products.Repositories
{
    public class ProductSchemaRepository : IProductSchemaRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;

        public ProductSchemaRepository(IApiClientService apiClientService,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration)
        {
            this.apiClientService = apiClientService;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
        }

        public async Task<Schema> GetProductSchemaByIdAsync(string token, string language, Guid? id)
        {
            var productSchemaRequestModel = new RequestModelBase
            {
                Id = id,
                Language = language
            };

            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = this.apiClientService.InitializeRequestModelContext(productSchemaRequestModel),
                AccessToken = token,
                EndpointAddress = this.servicesEndpointsConfiguration.Api.Host + this.servicesEndpointsConfiguration.Api.Endpoints.Schema
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, ProductSchemaResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return new Schema
                { 
                    Id = response.Data.Id,
                    Name = response.Data.Name,
                    JsonSchema = response.Data.JsonSchema,
                    UiSchema = response.Data.UiSchema
                };
            }

            return default;
        }

        public async Task<Schema> GetProductSchemaByEntityTypeIdAsync(string token, string language, Guid? id)
        {
            var productSchemaRequestModel = new RequestModelBase
            {
                Id = id,
                Language = language
            };

            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = this.apiClientService.InitializeRequestModelContext(productSchemaRequestModel),
                AccessToken = token,
                EndpointAddress = this.servicesEndpointsConfiguration.Api.Host + this.servicesEndpointsConfiguration.Api.Endpoints.SchemaByEntityType
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, ProductSchemaResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return new Schema
                {
                    Id = response.Data.Id,
                    Name = response.Data.Name,
                    JsonSchema = response.Data.JsonSchema,
                    UiSchema = response.Data.UiSchema
                };
            }

            return default;
        }
    }
}
