using Seller.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using System.Globalization;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class EditOrderPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, EditOrderPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, EditOrderFormViewModel> editOrderFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public EditOrderPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, EditOrderFormViewModel> editOrderFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.editOrderFormModelBuilder = editOrderFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<EditOrderPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new EditOrderPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                EditOrderForm = await this.editOrderFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
