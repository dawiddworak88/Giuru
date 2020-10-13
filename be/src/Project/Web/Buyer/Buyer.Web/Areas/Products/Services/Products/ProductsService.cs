using Buyer.Web.Areas.Products.ModelBuilders.Definitions;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Shared.Catalogs.ViewModels;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions;
using Foundation.Extensions.Services.MediaServices;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository productsRepository;
        private readonly IMediaService mediaService;
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;

        public ProductsService(
            IProductsRepository productsRepository,
            IMediaService mediaService,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator)
        {
            this.productsRepository = productsRepository;
            this.mediaService = mediaService;
            this.options = options;
            this.linkGenerator = linkGenerator;
        }

        public async Task<PagedResults<IEnumerable<CatalogItemViewModel>>> GetProductsAsync(Guid? categoryId, Guid? brandId, string language, string searchTerm, int pageIndex, int itemsPerPage, string token)
        {
            var catalogItemList = new List<CatalogItemViewModel>();

            if (categoryId.HasValue || brandId.HasValue)
            {
                var pagedProducts = await this.productsRepository.GetProductsAsync(null, categoryId, brandId, language, searchTerm, pageIndex, itemsPerPage, token);

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
                            BrandUrl = this.linkGenerator.GetPathByAction("Index", "Brand", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, product.BrandId }),
                            BrandName = product.BrandName,
                            InStock = false
                        };

                        if (product.Images != null)
                        {
                            var imageGuid = product.Images.FirstOrDefault();

                            catalogItem.ImageAlt = product.Name;
                            catalogItem.ImageUrl = this.mediaService.GetMediaUrl(this.options.Value.MediaUrl, imageGuid, ProductConstants.ProductsCatalogItemImageWidth, ProductConstants.ProductsCatalogItemImageHeight);
                        }

                        catalogItemList.Add(catalogItem);
                    }

                    return new PagedResults<IEnumerable<CatalogItemViewModel>>(pagedProducts.Total, pagedProducts.PageSize)
                    {
                        Data = catalogItemList
                    };
                }
            }

            return new PagedResults<IEnumerable<CatalogItemViewModel>>(catalogItemList.Count, PaginationConstants.DefaultPageIndex)
            {
                Data = catalogItemList
            };
        }
    }
}
