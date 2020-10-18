using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Shared.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryPageModelBuilder : IAsyncComponentModelBuilder<CategoryComponentModel, CategoryPageViewModel>
    {
        private readonly IModelBuilder<BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<CategoryComponentModel, CategoryCatalogViewModel> categoryCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public CategoryPageModelBuilder(
            IModelBuilder<BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<CategoryComponentModel, CategoryCatalogViewModel> categoryCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
            this.categoryCatalogModelBuilder = categoryCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<CategoryPageViewModel> BuildModelAsync(CategoryComponentModel componentModel)
        {

            var viewModel = new CategoryPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(new ComponentModelBase { Id = componentModel.Id, Token = componentModel.Token, IsAuthenticated = componentModel.IsAuthenticated, Language = componentModel.Language }),
                Catalog = await this.categoryCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
