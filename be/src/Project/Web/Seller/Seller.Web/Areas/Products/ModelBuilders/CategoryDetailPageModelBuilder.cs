using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Products.ViewModels;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class CategoryDetailPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CategoryDetailFormViewModel> categoryDetailFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public CategoryDetailPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CategoryDetailFormViewModel> categoryDetailFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.categoryDetailFormModelBuilder = categoryDetailFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<CategoryPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var categoryDetailFormComponentModel = new ComponentModelBase
            {
                Id = componentModel.Id,
                Token = componentModel.Token,
                Language = componentModel.Language
            };

            var viewModel = new CategoryPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                CategoryDetailForm = await categoryDetailFormModelBuilder.BuildModelAsync(categoryDetailFormComponentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
