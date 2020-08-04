using Seller.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Microsoft.Extensions.Localization;
using Feature.Order;
using System.Threading.Tasks;
using Seller.Web.Shared.ComponentModels;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class ImportOrderPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ImportOrderPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ImportOrderFormViewModel> importOrderFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        public ImportOrderPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ImportOrderFormViewModel> importOrderFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.importOrderFormModelBuilder = importOrderFormModelBuilder;
            this.orderLocalizer = orderLocalizer;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ImportOrderPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ImportOrderPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                ImportOrderForm = await importOrderFormModelBuilder.BuildModelAsync(new ComponentModelBase { Token = componentModel.Token }),
                Title = this.orderLocalizer["ImportOrder"],
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
