using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class OrdersPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrdersPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogOrderViewModel<Order>> ordersCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public OrdersPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogOrderViewModel<Order>> ordersCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.seoModelBuilder = seoModelBuilder;
            this.headerModelBuilder = headerModelBuilder;
            this.ordersCatalogModelBuilder = ordersCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
        }

        public async Task<OrdersPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrdersPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Metadata = await this.seoModelBuilder.BuildModelAsync(componentModel),
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(componentModel),
                Catalog = await this.ordersCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
