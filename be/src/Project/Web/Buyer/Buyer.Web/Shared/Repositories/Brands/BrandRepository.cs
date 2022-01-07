using Buyer.Web.Areas.Products.ApiResponseModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Buyer.Web.Shared.DomainModels.Brands;

namespace Buyer.Web.Shared.Repositories.Brands
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

        public async Task<Brand> GetBrandAsync(Guid? sellerId, string token, string language)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.SellersApiEndpoint}/{sellerId}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, BrandResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return new Brand
                {
                    Id = response.Data.Id.Value,
                    Name = response.Data.Name,
                    Description = response.Data.Description,
                    Files = response.Data.Files
                };
            }

            return default;
        }
    }
}
