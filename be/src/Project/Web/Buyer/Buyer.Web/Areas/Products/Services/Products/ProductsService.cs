using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Buyer.Web.Shared.Configurations;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Media.Services.MediaServices;
using System.Text.Json;
using System.Diagnostics;

namespace Buyer.Web.Areas.Products.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IMediaService _mediaService;
        private readonly IOptions<AppSettings> _options;
        private readonly LinkGenerator _linkGenerator;

        public ProductsService(
            IProductsRepository productsRepository,
            IMediaService mediaService,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator)
        {
            _productsRepository = productsRepository;
            _mediaService = mediaService;
            _options = options;
            _linkGenerator = linkGenerator;
        }

        public async Task<string> GetProductAttributesAsync(IEnumerable<ProductAttribute> productAttributes)
        {
            var attributesToDisplay = _options.Value.ProductAttributes.ToEnumerableString();

            var attributes = new List<string>();
            foreach (var productAttribute in attributesToDisplay.OrEmptyIfNull())
            {
                var existingAttribute = productAttributes.FirstOrDefault(x => x.Key == productAttribute);
                if (existingAttribute is not null)
                {
                    var attribute = string.Join(", ", existingAttribute.Values);

                    attributes.Add(attribute);
                }
            }

            return string.Join(", ", attributes.OrEmptyIfNull());
        }

        public async Task<PagedResults<IEnumerable<CatalogItemViewModel>>> GetProductsAsync(IEnumerable<Guid> ids, Guid? categoryId, Guid? sellerId, string language, string searchTerm, bool? hasPrimaryProduct, int pageIndex, int itemsPerPage, string token)
        {
            var catalogItemList = new List<CatalogItemViewModel>();
            var pagedProducts = await _productsRepository.GetProductsAsync(ids, categoryId, sellerId, language, searchTerm, hasPrimaryProduct, pageIndex, itemsPerPage, token, nameof(Product.Name));

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                pagedProducts.Data = pagedProducts.Data.Where(x => x.Name.StartsWith(searchTerm));
            }

            if (pagedProducts?.Data != null)
            {
                foreach (var product in pagedProducts.Data)
                {
                    var catalogItem = new CatalogItemViewModel
                    {
                        Id = product.Id,
                        Sku = product.Sku,
                        Title = product.Name,
                        Url = _linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, product.Id }),
                        BrandUrl = _linkGenerator.GetPathByAction("Index", "Brand", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = product.SellerId }),
                        BrandName = product.BrandName,
                        Images = product.Images,
                        InStock = false,
                        ProductAttributes = await GetProductAttributesAsync(product.ProductAttributes)
                    };

                    if (product.Images != null)
                    {
                        var imageGuid = product.Images.FirstOrDefault();

                        catalogItem.ImageAlt = product.Name;
                        catalogItem.ImageUrl = _mediaService.GetMediaUrl(imageGuid, ProductConstants.ProductsCatalogItemImageWidth);
                        catalogItem.Sources = new List<SourceViewModel>
                        {
                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(imageGuid, 1024) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(imageGuid, 352) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(imageGuid, 256) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(imageGuid, 768) },

                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(imageGuid, 1024) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(imageGuid, 352) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(imageGuid, 256) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(imageGuid, 768) }
                        };
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

        public async Task<IEnumerable<string>> GetProductSuggestionsAsync(string searchTerm, int size, string language, string token, string searchArea)
        {
            var products = await _productsRepository.GetProductsAsync(
                null,
                null,
                null,
                language,
                searchTerm,
                true,
                PaginationConstants.DefaultPageIndex,
                ProductConstants.ProductsCatalogPaginationPageSize,
                token,
                nameof(Product.Name));

            return products?.Data?.Select(x => x.Name);
        }
    }
}
