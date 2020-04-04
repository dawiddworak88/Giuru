using Feature.Account.ViewModels.SignInForm;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.Extensions.Localization;

namespace Feature.Account.ModelBuilders.SignInForm
{
    public class SignInFormModelBuilder : IModelBuilder<SignInFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<AccountResources> accountLocalizer;

        public SignInFormModelBuilder(IStringLocalizer<GlobalResources> globalLocalizer, IStringLocalizer<AccountResources> accountLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
            this.accountLocalizer = accountLocalizer;
        }

        public SignInFormViewModel BuildModel()
        {
            var viewModel = new SignInFormViewModel
            { 
                EmailFormatErrorMessage = this.globalLocalizer["EmailFormatErrorMessage"],
                EmailRequiredErrorMessage = this.globalLocalizer["EmailRequiredErrorMessage"],
                EnterEmailText = this.globalLocalizer["EnterEmailText"],
                EnterPasswordText = this.globalLocalizer["EnterPasswordText"],
                PasswordFormatErrorMessage = this.globalLocalizer["PasswordFormatErrorMessage"],
                PasswordRequiredErrorMessage = this.globalLocalizer["PasswordRequiredErrorMessage"],
                SignInText = this.accountLocalizer["SignInText"]
            };

            return viewModel;
        }
    }
}
