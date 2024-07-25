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
using Buyer.Web.Areas.Products.Services.CompletionDates;
using IdentityServer4.Extensions;

namespace Buyer.Web.Areas.Products.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IMediaService _mediaService;
        private readonly IOptions<AppSettings> _options;
        private readonly LinkGenerator _linkGenerator;
        private readonly ICompletionDatesService _completionDatesService;

        public ProductsService(
            IProductsRepository productsRepository,
            IMediaService mediaService,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator,
            ICompletionDatesService completionDatesService)
        {
            _productsRepository = productsRepository;
            _mediaService = mediaService;
            _options = options;
            _linkGenerator = linkGenerator;
            _completionDatesService = completionDatesService;
        }

        public async Task<Product> GetProductAsync(string token, string language, Guid? productId, Guid? sellerId)
        {
            var product = await _productsRepository.GetProductAsync(productId, language, token);

            if (product is not null)
            {
                if (String.IsNullOrWhiteSpace(_options.Value.CompletionDatesUrl) is false)
                {
                    product = await _completionDatesService.GetCompletionDateAsync(token, language, sellerId, product);
                }

                return product;
            }

            return default;
        }

        public async Task<string> GetProductAttributesAsync(IEnumerable<ProductAttribute> productAttributes)
        {
            if (productAttributes is null)
            {
                return default;
            }

            var attributesToDisplay = _options.Value.ProductAttributes.ToEnumerableString();

            var attributes = new List<string>();
            foreach(var productAttribute in attributesToDisplay.OrEmptyIfNull())
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

            if (pagedProducts?.Data is not null)
            {
                if (String.IsNullOrWhiteSpace(_options.Value.CompletionDatesUrl) is false)
                {
                    pagedProducts.Data = await _completionDatesService.GetCompletionDatesAsync(token, language, sellerId, pagedProducts.Data);
                }

                foreach (var product in pagedProducts.Data.OrEmptyIfNull())
                {
                    var catalogItem = new CatalogItemViewModel
                    {
                        Id = product.Id,
                        Sku = product.Sku,
                        Title = product.Name,
                        CompletionDate = product.CompletionDate,
                        Url = _linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, product.Id }),
                        BrandUrl = _linkGenerator.GetPathByAction("Index", "Brand", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = product.SellerId }),
                        BrandName = product.BrandName,
                        Images = product.Images,
                        InStock = false,
                        ProductAttributes = await GetProductAttributesAsync(product.ProductAttributes)
                    };

                    if (product.Images is not null)
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

        public async Task<IEnumerable<string>> GetProductSuggestionsAsync(string searchTerm, int size, string language, string token)
        {
            return await _productsRepository.GetProductSuggestionsAsync(searchTerm, size, language, token);
        }
    }
}
