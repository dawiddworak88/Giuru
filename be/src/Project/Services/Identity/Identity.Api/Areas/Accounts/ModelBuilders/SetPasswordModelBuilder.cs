using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.ViewModels;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ModelBuilders
{
    public class SetPasswordModelBuilder : IAsyncComponentModelBuilder<SetPasswordComponentModel, SetPasswordViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<SetPasswordFormComponentModel, SetPasswordFormViewModel> signPasswordFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public SetPasswordModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<SetPasswordFormComponentModel, SetPasswordFormViewModel> signPasswordFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.signPasswordFormModelBuilder = signPasswordFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<SetPasswordViewModel> BuildModelAsync(SetPasswordComponentModel componentModel)
        {
            var viewModel = new SetPasswordViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                SetPasswordForm = await this.signPasswordFormModelBuilder.BuildModelAsync(new SetPasswordFormComponentModel { Id = componentModel.Id, ReturnUrl = componentModel.ReturnUrl }),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}