using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class StatusOrderPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, StatusOrderPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, StatusOrderFormViewModel> editOrderFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public StatusOrderPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, StatusOrderFormViewModel> editOrderFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.editOrderFormModelBuilder = editOrderFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
        }

        public async Task<StatusOrderPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new StatusOrderPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(componentModel),
                StatusOrder = await this.editOrderFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
