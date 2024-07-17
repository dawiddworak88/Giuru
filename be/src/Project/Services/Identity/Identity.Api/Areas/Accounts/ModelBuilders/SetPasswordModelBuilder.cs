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
        private readonly IModelBuilder<HeaderViewModel> _headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<SetPasswordFormComponentModel, SetPasswordFormViewModel> _signPasswordFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public SetPasswordModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<SetPasswordFormComponentModel, SetPasswordFormViewModel> signPasswordFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _signPasswordFormModelBuilder = signPasswordFormModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<SetPasswordViewModel> BuildModelAsync(SetPasswordComponentModel componentModel)
        {
            var viewModel = new SetPasswordViewModel
            {
                Header = _headerModelBuilder.BuildModel(),
                SetPasswordForm = await _signPasswordFormModelBuilder.BuildModelAsync(new SetPasswordFormComponentModel
                {
                    Id = componentModel.Id,
                    ReturnUrl = componentModel.ReturnUrl,
                    Language = componentModel.Language,
                    Token = componentModel.Token
                }),
                Footer = _footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}