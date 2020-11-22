using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.Configurations;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Shared.Definitions;

namespace Seller.Web.Areas.Products.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public ProductsRepository(IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = this.apiClientService.InitializeRequestModelContext(new RequestModelBase()),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, PagedResults<IEnumerable<Product>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                var products = new List<Product>();

                foreach (var productResponse in response.Data.Data)
                {
                    var product = new Product
                    {
                        Id = productResponse.Id,
                        PrimaryProductId = productResponse.PrimaryProductId,
                        Sku = productResponse.Sku,
                        Name = productResponse.Name,
                        Description = productResponse.Description,
                        IsNew = productResponse.IsNew,
                        IsProtected = productResponse.IsProtected,
                        FormData = productResponse.FormData,
                        SellerId = productResponse.SellerId,
                        BrandName = productResponse.BrandName,
                        CategoryId = productResponse.CategoryId,
                        CategoryName = productResponse.CategoryName,
                        ProductVariants = productResponse.ProductVariants,
                        Images = productResponse.Images,
                        Files = productResponse.Files,
                        Videos = productResponse.Videos
                    };

                    products.Add(product);
                }

                return new PagedResults<IEnumerable<Product>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = products
                };
            }

            return default;
        }
    }
}
