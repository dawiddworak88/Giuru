using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ModelBuilders.Definitions;
using Buyer.Web.Areas.Products.Repositories.Categories;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Shared.Catalogs.ViewModels;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryCatalogModelBuilder : IAsyncComponentModelBuilder<CategoryComponentModel, CategoryCatalogViewModel>
    {
        private readonly IProductsRepository productsRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IMediaService mediaService;
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;

        public CategoryCatalogModelBuilder(
            IProductsRepository productsRepository,
            ICategoryRepository categoryRepository,
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<ProductResources> productLocalizer,
            IMediaService mediaService,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator)
        {
            this.productsRepository = productsRepository;
            this.categoryRepository = categoryRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.mediaService = mediaService;
            this.options = options;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CategoryCatalogViewModel> BuildModelAsync(CategoryComponentModel componentModel)
        {
            var viewModel = new CategoryCatalogViewModel
            {
                SkuLabel = this.productLocalizer.GetString("Sku"),
                SignInUrl = "#",
                SignInToSeePricesLabel = this.globalLocalizer.GetString("SignInToSeePrices"),
                ResultsLabel = this.globalLocalizer.GetString("Results"),
                ByLabel = this.globalLocalizer.GetString("By"),
                InStockLabel = this.productLocalizer.GetString("InStock"),
                NoResultsLabel = this.globalLocalizer.GetString("NoResults"),
                GeneralErrorMessage = this.globalLocalizer["AnErrorOccurred"],
                DisplayedRowsLabel = this.globalLocalizer["DisplayedRows"],
                RowsPerPageLabel = this.globalLocalizer["RowsPerPage"],
                BackIconButtonText = this.globalLocalizer["Previous"],
                NextIconButtonText = this.globalLocalizer["Next"],
                IsAuthenticated = componentModel.IsAuthenticated
            };

            var category = await this.categoryRepository.GetCategoryAsync(componentModel.Id, componentModel.Token);

            if (category != null)
            {
                viewModel.Title = category.Name;
                viewModel.Id = category.Id;

                var pagedProducts = await this.productsRepository.GetProductsAsync(
                    componentModel.Id,
                    componentModel.Language,
                    componentModel.SearchTerm,
                    PaginationConstants.DefaultPageIndex,
                    CategoryConstants.CategoryCatalogPaginationPageSize,
                    componentModel.Token);

                var catalogItemList = new List<CatalogItemViewModel>();

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
                            BrandUrl = this.linkGenerator.GetPathByAction("Index", "Brand", new { Area = "Brands", culture = CultureInfo.CurrentUICulture.Name, product.BrandId }),
                            BrandName = product.BrandName,
                            InStock = false
                        };

                        if (product.Images != null && product.Images.Any())
                        {
                            var imageGuid = product.Images.FirstOrDefault();

                            if (imageGuid != null)
                            {
                                catalogItem.ImageAlt = product.Name;
                                catalogItem.ImageUrl = this.mediaService.GetMediaUrl(this.options.Value.MediaUrl, imageGuid, CategoryConstants.CategoryCatalogItemImageWidth, CategoryConstants.CategoryCatalogItemImageHeight);
                            }
                        }

                        catalogItemList.Add(catalogItem);
                    }

                    viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(pagedProducts.Total, pagedProducts.PageSize)
                    {
                        Data = catalogItemList
                    };
                }
                else
                {
                    viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(catalogItemList.Count, PaginationConstants.DefaultPageSize)
                    {
                        Data = catalogItemList
                    };
                }
            }

            return viewModel;
        }
    }
}
