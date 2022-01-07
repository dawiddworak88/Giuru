using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Buyer.Web.Shared.Configurations;
using Foundation.Extensions.Services.MediaServices;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;

namespace Buyer.Web.Areas.Products.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository productsRepository;
        private readonly IMediaHelperService mediaService;
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;
        private readonly ICdnService cdnService;

        public ProductsService(
            IProductsRepository productsRepository,
            IMediaHelperService mediaService,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator,
            ICdnService cdnService)
        {
            this.productsRepository = productsRepository;
            this.mediaService = mediaService;
            this.options = options;
            this.linkGenerator = linkGenerator;
            this.cdnService = cdnService;
        }

        public async Task<PagedResults<IEnumerable<CatalogItemViewModel>>> GetProductsAsync(IEnumerable<Guid> ids, Guid? categoryId, Guid? sellerId, string language, string searchTerm, int pageIndex, int itemsPerPage, string token)
        {
            var catalogItemList = new List<CatalogItemViewModel>();

            var pagedProducts = await this.productsRepository.GetProductsAsync(ids, categoryId, sellerId, language, searchTerm, pageIndex, itemsPerPage, token, nameof(Product.Name));

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
                        InStock = false
                    };

                    if (product.Images != null)
                    {
                        var imageGuid = product.Images.FirstOrDefault();

                        catalogItem.ImageAlt = product.Name;
                        catalogItem.ImageUrl = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, imageGuid, ProductConstants.ProductsCatalogItemImageWidth, ProductConstants.ProductsCatalogItemImageHeight));
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

        public async Task<IEnumerable<string>> GetProductSuggestionsAsync(string searchTerm, int size, string language, string token)
        {
            return await this.productsRepository.GetProductSuggestionsAsync(searchTerm, size, language, token);
        }
    }
}
