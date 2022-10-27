using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ViewModels;
using Seller.Web.Areas.Products.ComponentModels;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class DuplicateCategoryPageModelBuilder : IAsyncComponentModelBuilder<DuplicateCategoryComponentModel, CategoryPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<DuplicateCategoryComponentModel, CategoryFormViewModel> duplicateCategoryFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public DuplicateCategoryPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<DuplicateCategoryComponentModel, CategoryFormViewModel> duplicateCategoryFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.duplicateCategoryFormModelBuilder = duplicateCategoryFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<CategoryPageViewModel> BuildModelAsync(DuplicateCategoryComponentModel componentModel)
        {
            var viewModel = new CategoryPageViewModel
            {
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                CategoryForm = await this.duplicateCategoryFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
