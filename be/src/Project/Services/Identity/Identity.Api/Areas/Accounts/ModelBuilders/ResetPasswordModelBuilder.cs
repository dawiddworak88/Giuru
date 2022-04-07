using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.ViewModels;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ModelBuilders
{
    public class ResetPasswordModelBuilder : IAsyncComponentModelBuilder<ResetPasswordComponentModel, ResetPasswordViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ResetPasswordComponentModel, ResetPasswordFormViewModel> resetPasswordFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public ResetPasswordModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ResetPasswordComponentModel, ResetPasswordFormViewModel> resetPasswordFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.resetPasswordFormModelBuilder = resetPasswordFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ResetPasswordViewModel> BuildModelAsync(ResetPasswordComponentModel componentModel)
        {
            var viewModel = new ResetPasswordViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                ResetPasswordForm = await this.resetPasswordFormModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}