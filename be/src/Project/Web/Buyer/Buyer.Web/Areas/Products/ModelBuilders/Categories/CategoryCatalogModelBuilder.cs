using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Repositories.Categories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using System.Threading.Tasks;
using Foundation.PageContent.ComponentModels;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Foundation.Extensions.ExtensionMethods;
using System.Linq;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using System.Globalization;
using Microsoft.AspNetCore.Routing;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryCatalogModelBuilder : IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> catalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder;
        private readonly IProductsService productsService;
        private readonly ICategoryRepository categoryRepository;
        private readonly IBasketRepository basketRepository;
        private readonly LinkGenerator linkGenerator;

        public CategoryCatalogModelBuilder(
            ICatalogModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> catalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IProductsService productsService,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator,
            ICategoryRepository categoryRepository)
        {
            this.basketRepository = basketRepository;
            this.catalogModelBuilder = catalogModelBuilder;
            this.sidebarModelBuilder = sidebarModelBuilder;
            this.productsService = productsService;
            this.categoryRepository = categoryRepository;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CategoryCatalogViewModel> BuildModelAsync(SearchProductsComponentModel componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel(componentModel);

            var category = await this.categoryRepository.GetCategoryAsync(componentModel.Id, componentModel.Token, componentModel.Language);

            if (category != null)
            {
                viewModel.Title = category.Name;
                viewModel.CategoryId = category.Id;
                viewModel.Sidebar = await this.sidebarModelBuilder.BuildModelAsync(componentModel);
                viewModel.PagedItems = await this.productsService.GetProductsAsync(
                    null,
                    componentModel.Id,
                    null,
                    componentModel.Language,
                    componentModel.SearchTerm,
                    PaginationConstants.DefaultPageIndex,
                    ProductConstants.ProductsCatalogPaginationPageSize,
                    componentModel.Token);

                if (componentModel.IsAuthenticated && componentModel.BasketId.HasValue)
                {
                    var existingBasket = await this.basketRepository.GetBasketById(componentModel.Token, componentModel.Language, componentModel.BasketId.Value);
                    
                    if (existingBasket != null)
                    {
                        var productIds = existingBasket.Items.OrEmptyIfNull().Select(x => x.ProductId.Value);
                        if (productIds.OrEmptyIfNull().Any())
                        {
                            var basketResponseModel = existingBasket.Items.OrEmptyIfNull().Select(x => new BasketItemResponseModel
                            {
                                ProductId = x.ProductId,
                                ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                                Name = x.ProductName,
                                Sku = x.ProductSku,
                                Quantity = x.Quantity,
                                ExternalReference = x.ExternalReference,
                                ImageSrc = x.PictureUrl,
                                ImageAlt = x.ProductName,
                                DeliveryFrom = x.DeliveryFrom,
                                DeliveryTo = x.DeliveryTo,
                                MoreInfo = x.MoreInfo
                            });

                            viewModel.OrderItems = basketResponseModel;
                        }
                    }
                }
            }

            return viewModel;
        }
    }
}
