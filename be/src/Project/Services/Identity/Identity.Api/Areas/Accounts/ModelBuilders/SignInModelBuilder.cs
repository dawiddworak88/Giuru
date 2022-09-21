using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.ViewModels;
using Identity.Api.ViewModels.SignInForm;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;

namespace Identity.Api.Areas.Accounts.ModelBuilders
{
    public class SignInModelBuilder : IComponentModelBuilder<SignInComponentModel, SignInViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IComponentModelBuilder<SignInFormComponentModel, SignInFormViewModel> signInFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public SignInModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IComponentModelBuilder<SignInFormComponentModel, SignInFormViewModel> signInFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.signInFormModelBuilder = signInFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public SignInViewModel BuildModel(SignInComponentModel componentModel)
        {
            var signInFormComponentModel = new SignInFormComponentModel
            {
                ReturnUrl = componentModel.ReturnUrl,
                DevelopersEmail = componentModel.DevelopersEmail,
                ErrorMessage = componentModel.ErrorMessage
            };

            var viewModel = new SignInViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                SignInForm = signInFormModelBuilder.BuildModel(signInFormComponentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
