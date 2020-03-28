using Account.Areas.Accounts.ViewModels;
using Feature.Account.ViewModels.SignInForm;
using Feature.PageContent.Shared.Footers.ViewModels;
using Feature.PageContent.Shared.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;

namespace Account.Areas.Accounts.ModelBuilders
{
    public class SignInModelBuilder : IModelBuilder<SignInViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;

        private readonly IModelBuilder<SignInFormViewModel> signInFormModelBuilder;

        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public SignInModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<SignInFormViewModel> signInFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.signInFormModelBuilder = signInFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public SignInViewModel BuildModel()
        {
            var viewModel = new SignInViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                SignInForm = signInFormModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
