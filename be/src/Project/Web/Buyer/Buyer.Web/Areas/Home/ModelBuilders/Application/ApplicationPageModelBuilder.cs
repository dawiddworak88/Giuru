using Buyer.Web.Areas.Home.ViewModel.Application;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders.Application
{
    public class ApplicationPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ApplicationPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ApplicationFormViewModel> applicationFormModelBuilder;

        public ApplicationPageModelBuilder(
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
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
                Header = headerModelBuilder.BuildModel(),
                ApplicationForm = await this.applicationFormModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
