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
using Buyer.Web.Areas.Products.Services.ProductColors;
using Foundation.Search.Paginations;
using Foundation.Search.Models;

namespace Buyer.Web.Areas.Products.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository productsRepository;
        private readonly IMediaService mediaService;
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;
        private readonly IProductColorsService productColorsService;

        public ProductsService(
            IProductsRepository productsRepository,
            IMediaService mediaService,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator,
            IProductColorsService productColorsService)
        {
            this.productsRepository = productsRepository;
            this.mediaService = mediaService;
            this.options = options;
            this.linkGenerator = linkGenerator;
            this.productColorsService = productColorsService;
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

        public async Task<PagedResults<IEnumerable<CatalogItemViewModel>>> GetProductsAsync(IEnumerable<Guid> ids, Guid? categoryId, Guid? sellerId, string language, string searchTerm, bool? hasPrimaryProduct, int pageIndex, int itemsPerPage, string token, string orderBy)
        {
            var catalogItemList = new List<CatalogItemViewModel>();
            
            var pagedProducts = await this.productsRepository.GetProductsAsync(ids, categoryId, sellerId, language, searchTerm, hasPrimaryProduct, pageIndex, itemsPerPage, token, orderBy);

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
                        ExtraPacking = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleExtraPackingAttributeKeys),
                        PaletteSize = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossiblePaletteSizeAttributeKeys),
                        Size = GetSize(product.ProductAttributes),
                        PointsOfLight = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossiblePointsOfLightAttributeKeys),
                        LampshadeType = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleLampshadeTypeAttributeKeys),
                        LampshadeSize = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleLampshadeSizeAttributeKeys),
                        LinearLight = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleLinearLightAttributeKeys).ToYesOrNo(),
                        Mirror = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleMirrorAttributeKeys).ToYesOrNo(),
                        Shape = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleShapeAttributeKeys),
                        PrimaryColor = await this.productColorsService.ToEnglishAsync(GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossiblePrimaryColorAttributeKeys)),
                        SecondaryColor = await this.productColorsService.ToEnglishAsync(GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleSecondaryColorAttributeKeys)),
                        ShelfType = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleShelfTypeAttributeKeys)
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

        public string GetFirstAvailableAttributeValue(IEnumerable<ProductAttribute> attributes, string possibleKeys)
        {
            var keys = possibleKeys.ToEnumerableString();

            foreach (var key in keys.OrEmptyIfNull())
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

        public string GetSize(IEnumerable<ProductAttribute> attributes)
        {
            var widthValue = GetFirstAvailableAttributeValue(attributes, this.options.Value.PossibleWidthAttributeKeys);
            var depthValue = GetFirstAvailableAttributeValue(attributes, this.options.Value.PossibleDepthAttributeKeys);
            var lengthValue = GetFirstAvailableAttributeValue(attributes, this.options.Value.PossibleLengthAttributeKeys);

            if (string.IsNullOrWhiteSpace(widthValue))
            {
                return default;
            }

            if (string.IsNullOrWhiteSpace(depthValue) is false)
            {
                var size = $"{widthValue}x{depthValue}".Trim();
                return size;
            }

            if (string.IsNullOrWhiteSpace(lengthValue) is false)
            {
                var size = $"{widthValue}x{lengthValue}".Trim();
                return size;
            }

            return default;
        }

        public async Task<PagedResultsWithFilters<IEnumerable<CatalogItemViewModel>>> GetProductsAsync(
            string token, 
            string language, 
            string searchTerm, 
            int pageIndex, 
            int itemsPerPage, 
            string source, 
            string orderBy, 
            QueryFilters filters)
        {
            var pagedProducts = await this.productsRepository.GetProductsWithFiltersAsync(token, language, null, searchTerm, pageIndex, itemsPerPage, source, orderBy, filters);

            if (pagedProducts?.Data != null)
            {
                var catalogItemList = new List<CatalogItemViewModel>();

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
                        ExtraPacking = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleExtraPackingAttributeKeys),
                        PaletteSize = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossiblePaletteSizeAttributeKeys),
                        Size = GetSize(product.ProductAttributes),
                        PointsOfLight = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossiblePointsOfLightAttributeKeys),
                        LampshadeType = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleLampshadeTypeAttributeKeys),
                        LampshadeSize = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleLampshadeSizeAttributeKeys),
                        LinearLight = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleLinearLightAttributeKeys).ToYesOrNo(),
                        Mirror = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleMirrorAttributeKeys).ToYesOrNo(),
                        Shape = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleShapeAttributeKeys),
                        PrimaryColor = await this.productColorsService.ToEnglishAsync(GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossiblePrimaryColorAttributeKeys)),
                        SecondaryColor = await this.productColorsService.ToEnglishAsync(GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleSecondaryColorAttributeKeys)),
                        ShelfType = GetFirstAvailableAttributeValue(product.ProductAttributes, this.options.Value.PossibleShelfTypeAttributeKeys)
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

                return new PagedResultsWithFilters<IEnumerable<CatalogItemViewModel>>(pagedProducts.Total, pagedProducts.PageSize)
                {
                    Data = catalogItemList,
                    Filters = pagedProducts.Filters
                };
            }

            return new PagedResultsWithFilters<IEnumerable<CatalogItemViewModel>>(pagedProducts.Total, PaginationConstants.DefaultPageIndex)
            {
                Data = Enumerable.Empty<CatalogItemViewModel>(),
                Filters = new List<Filter>()
            };
        }
    }
}
