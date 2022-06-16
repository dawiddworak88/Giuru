using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Buyer.Web.Shared.ApiRequestModels.Catalogs;
using Foundation.GenericRepository.Paginations;
using System.Linq;
using Buyer.Web.Shared.DomainModels.Categories;
using System;
using Buyer.Web.Shared.ViewModels.Catalogs;
using System.Globalization;
using Microsoft.AspNetCore.Routing;
using Buyer.Web.Shared.Repositories.Products;
using Buyer.Web.Shared.DomainModels.CatalogProducts;
using Foundation.Media.Services.MediaServices;

namespace Buyer.Web.Shared.Services.Catalogs
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogProductsRepository catalogProductsRepository;
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;
        private readonly IMediaService mediaService;
        private readonly LinkGenerator linkGenerator;

        public CatalogService(
            ICatalogProductsRepository catalogProductsRepository,
            IApiClientService apiClientService, 
            IOptions<AppSettings> settings,
            IMediaService mediaService,
            LinkGenerator linkGenerator)
        {
            this.catalogProductsRepository = catalogProductsRepository;
            this.apiClientService = apiClientService;
            this.settings = settings;
            this.mediaService = mediaService;
            this.linkGenerator = linkGenerator;
        }

        public async Task<PagedResults<IEnumerable<CatalogItemViewModel>>> GetCatalogProductsAsync(
            string token,
            string language,
            Guid? sellerId,
            bool? hasPrimaryProduct,
            bool? isNew,
            string searchTerm, 
            int pageIndex, 
            int itemsPerPage)
        {
            var catalogItemList = new List<CatalogItemViewModel>();

            var pagedProducts = await this.catalogProductsRepository.GetProductsAsync(
                    token,
                    language,
                    searchTerm,
                    hasPrimaryProduct,
                    isNew,
                    null,
                    pageIndex,
                    itemsPerPage,
                    $"{nameof(CatalogProduct.LastModifiedDate)} desc"
                );

            if (pagedProducts?.Data != null)
            {
                foreach (var product in pagedProducts.Data)
                {
                    var catalogItem = new CatalogItemViewModel
                    {
                        Id = product.Id,
                        Sku = product.Sku,
                        Title = product.Name,
                        Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, product.Id }),
                        BrandUrl = this.linkGenerator.GetPathByAction("Index", "Brand", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = product.SellerId }),
                        BrandName = product.BrandName,
                        IsNew = product.IsNew,
                        Images = product.Images,
                        InStock = false
                    };

                    if (product.Images != null)
                    {
                        var imageGuid = product.Images.FirstOrDefault();

                        catalogItem.ImageAlt = product.Name;
                        catalogItem.ImageUrl = this.mediaService.GetMediaUrl(imageGuid, 1920);
                    }

                    catalogItemList.Add(catalogItem);
                }

                return new PagedResults<IEnumerable<CatalogItemViewModel>>(pagedProducts.Total, pagedProducts.PageSize)
                {
                    Data = catalogItemList
                };
            }

            return new PagedResults<IEnumerable<CatalogItemViewModel>>(catalogItemList.Count, PaginationConstants.DefaultPageIndex)
            {
                Data = catalogItemList
            };
        }

        public async Task<IEnumerable<CatalogCategory>> GetCatalogCategoriesAsync(
            string language, 
            int pageIndex, 
            int itemsPerPage,
            string orderBy)
        {
            var categoriesRequestModel = new CategoriesRequestModel
            {
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<CategoriesRequestModel>
            {
                Language = language,
                Data = categoriesRequestModel,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.CategoriesApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<CategoriesRequestModel>, CategoriesRequestModel, PagedResults<IEnumerable<CatalogCategory>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null && response.Data.Data.Count() > 0)
            {
                var categories = new List<CatalogCategory>();

                categories.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)itemsPerPage);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await this.apiClientService.GetAsync<ApiRequest<CategoriesRequestModel>, CategoriesRequestModel, PagedResults<IEnumerable<CatalogCategory>>>(apiRequest);

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Count() > 0)
                    {
                        categories.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return categories;
            }

            return default;
        }
    }
}
