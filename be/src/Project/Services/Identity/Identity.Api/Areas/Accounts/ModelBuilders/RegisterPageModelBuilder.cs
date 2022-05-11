using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Identity.Api.Areas.Accounts.ViewModels;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ModelBuilders
{
    public class RegisterPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, RegisterPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public RegisterPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }
        public async Task<RegisterPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new RegisterPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
