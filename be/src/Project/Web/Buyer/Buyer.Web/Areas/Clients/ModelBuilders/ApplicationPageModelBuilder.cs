using Buyer.Web.Areas.Clients.ViewModels;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Clients.ModelBuilders
{
    public class ApplicationPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ApplicationPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ApplicationFormViewModel> applicationFormModelBuilder;

        public ApplicationPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ApplicationFormViewModel> applicationFormModelBuilder)
        {
            this.footerModelBuilder = footerModelBuilder;
            this.applicationFormModelBuilder = applicationFormModelBuilder;
            this.headerModelBuilder = headerModelBuilder;
        }
        public async Task<ApplicationPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ApplicationPageViewModel
            {
                Header = await headerModelBuilder.BuildModelAsync(componentModel),
                ApplicationForm = await applicationFormModelBuilder.BuildModelAsync(componentModel),
                Footer = await footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
