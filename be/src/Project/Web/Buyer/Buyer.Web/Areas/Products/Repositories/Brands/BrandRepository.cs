using Buyer.Web.Areas.Products.ApiResponseModels;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.Brands
{
    public class BrandRepository : IBrandRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public BrandRepository(IApiClientService apiClientService, IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<Brand> GetBrandAsync(Guid? sellerId, string token)
        {
            var productsRequestModel = new RequestModelBase
            {
                Id = sellerId
            };

            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = this.apiClientService.InitializeRequestModelContext(productsRequestModel),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Seller.SellerApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, BrandResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return new Brand
                {
                    Id = response.Data.Id,
                    Name = response.Data.Name,
                    Description = response.Data.Description,
                    Files = response.Data.Files
                };
            }

            return default;
        }
    }
}
