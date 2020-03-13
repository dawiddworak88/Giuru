using Feature.Account.ViewModels.SignInForm;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;

namespace Feature.Account.ModelBuilders.SignInForm
{
    public class SignInFormModelBuilder : IModelBuilder<SignInFormViewModel>
    {
        private readonly IStringLocalizer<AccountResources> accountLocalizer;

        public SignInFormModelBuilder(IStringLocalizer<AccountResources> accountLocalizer)
        {
            this.accountLocalizer = accountLocalizer;
        }

        public SignInFormViewModel BuildModel()
        {
            var viewModel = new SignInFormViewModel
            { 
                EmailFormatErrorMessage = this.accountLocalizer["EmailFormatErrorMessage"],
                EmailRequiredErrorMessage = this.accountLocalizer["PasswordRequiredErrorMessage"],
                EnterEmailText = this.accountLocalizer["EnterEmailText"],
                EnterPasswordText = this.accountLocalizer["EnterPasswordText"],
                PasswordFormatErrorMessage = this.accountLocalizer["PasswordFormatErrorMessage"],
                PasswordRequiredErrorMessage = this.accountLocalizer["PasswordRequiredErrorMessage"],
                SignInText = this.accountLocalizer["SignInText"]
            };

            return viewModel;
        }
    }
}
