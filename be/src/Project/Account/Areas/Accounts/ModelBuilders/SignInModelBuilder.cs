using Account.Areas.Accounts.ViewModels;
using Account.Shared.Footers.ViewModels;
using Account.Shared.Headers.ViewModels;
using Feature.Localization;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;

namespace Account.Areas.Accounts.ModelBuilders
{
    public class SignInModelBuilder : IModelBuilder<SignInViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;

        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public SignInModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.globalLocalizer = globalLocalizer;
        }

        public SignInViewModel BuildModel()
        {
            var viewModel = new SignInViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
