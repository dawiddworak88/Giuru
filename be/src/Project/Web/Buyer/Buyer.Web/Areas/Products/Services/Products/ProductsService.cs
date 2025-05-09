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

        public async Task<string> GetProductAttributesAsync(IEnumerable<ProductAttribute> productAttributes)
        {
            var attributesToDisplay = this.options.Value.ProductAttributes.ToEnumerableString();

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

            var pagedProducts = await this.productsRepository.GetProductsAsync(ids, categoryId, sellerId, language, searchTerm, hasPrimaryProduct, pageIndex, itemsPerPage, token, nameof(Product.Name));

            if (pagedProducts?.Data != null)
            {
                foreach (var product in pagedProducts.Data.OrEmptyIfNull())
                {
                    var catalogItem = new CatalogItemViewModel
                    {
                        Id = product.Id,
                        PrimaryProductSku = product.PrimaryProductSku,
                        Sku = product.Sku,
                        Title = product.Name,
                        Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, product.Id }),
                        BrandUrl = this.linkGenerator.GetPathByAction("Index", "Brand", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = product.SellerId }),
                        BrandName = product.BrandName,
                        Images = product.Images,
                        InStock = false,
                        ProductAttributes = await this.GetProductAttributesAsync(product.ProductAttributes),
                        SleepAreaSize = GetSleepAreaSize(product.ProductAttributes),
                        FabricsGroup = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossiblePriceGroupAttributeKeys),
                        ExtraPacking = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleExtraPackingAttributeKeys)
                    };

                    if (product.Images != null)
                    {
                        var imageGuid = product.Images.FirstOrDefault();

                        catalogItem.ImageAlt = product.Name;
                        catalogItem.ImageUrl = this.mediaService.GetMediaUrl(imageGuid, ProductConstants.ProductsCatalogItemImageWidth);
                        catalogItem.Sources = new List<SourceViewModel>
                        {
                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(imageGuid, 1024) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(imageGuid, 352) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(imageGuid, 256) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(imageGuid, 768) },

                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(imageGuid, 1024) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(imageGuid, 352) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(imageGuid, 256) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(imageGuid, 768) }
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
            return await this.productsRepository.GetProductSuggestionsAsync(searchTerm, size, language, token);
        }

        public string GetFirstAvailableAttributeValue(IEnumerable<ProductAttribute> attributes, params string[] possibleKeys)
        {
            foreach (var key in possibleKeys.OrEmptyIfNull())
            {
                var value = attributes.FirstOrDefault(x => x.Key == key)?.Values?.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(value) is false)
                {
                    return value;
                }
            }

            return null;
        }

        public string GetSleepAreaSize(IEnumerable<ProductAttribute> attributes)
        {
            var sleepAreaWidthValue = GetFirstAvailableAttributeValue(attributes, this.options.Value.PossibleSleepAreaWidthAttributeKeys);
            var sleepAreaDepthValue = GetFirstAvailableAttributeValue(attributes, this.options.Value.PossibleSleepAreaDepthAttributeKeys);

            if (string.IsNullOrWhiteSpace(sleepAreaWidthValue) ||
                string.IsNullOrWhiteSpace(sleepAreaDepthValue))
            {
                return default;
            }

            var size = $"{sleepAreaWidthValue}x{sleepAreaDepthValue}".Trim();

            return size;
        }

    }
}
