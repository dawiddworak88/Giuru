using Tenant.Portal.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Tenant.Portal.Shared.Headers.ViewModels;
using Microsoft.Extensions.Localization;
using Feature.Localization;
using Tenant.Portal.Shared.Footers.ViewModels;

namespace Tenant.Portal.Areas.Orders.ModelBuilders
{
    public class OrderPageModelBuilder : IModelBuilder<OrderPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;

        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public OrderPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.globalLocalizer = globalLocalizer;
        }

        public OrderPageViewModel BuildModel()
        {
            var viewModel = new OrderPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel(),
                Welcome = this.globalLocalizer["Close"],
                LearnMore = "Learn more"
            };

            return viewModel;
        }
    }
}
