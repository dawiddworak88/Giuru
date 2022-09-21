using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.ViewModels.SignInForm;
using Feature.Account;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Identity.Api.Configurations;
using Identity.Api.Areas.Accounts.Definitions;

namespace Identity.Api.ModelBuilders.SignInForm
{
    public class SignInFormModelBuilder : IComponentModelBuilder<SignInFormComponentModel, SignInFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<AccountResources> accountLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IOptions<AppSettings> options;

        public SignInFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<AccountResources> accountLocalizer,
            IOptions<AppSettings> options,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.accountLocalizer = accountLocalizer;
            this.linkGenerator = linkGenerator;
            this.options = options;
        }

        public SignInFormViewModel BuildModel(SignInFormComponentModel componentModel)
        {
            var viewModel = new SignInFormViewModel
            {
                EmailFormatErrorMessage = this.globalLocalizer["EmailFormatErrorMessage"],
                EmailRequiredErrorMessage = this.globalLocalizer["EmailRequiredErrorMessage"],
                EnterEmailText = this.globalLocalizer["EnterEmailText"],
                EnterPasswordText = this.globalLocalizer["EnterPasswordText"],
                PasswordFormatErrorMessage = this.globalLocalizer["PasswordFormatErrorMessage"],
                PasswordRequiredErrorMessage = this.globalLocalizer["PasswordRequiredErrorMessage"],
                SignInText = this.accountLocalizer["SignInText"],
                SubmitUrl = this.linkGenerator.GetPathByAction("Index", "SignIn", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name }),
                ReturnUrl = componentModel.ReturnUrl,
                ForgotPasswordLabel = this.accountLocalizer.GetString("ForgotPasswordLabel"),
                ResetPasswordUrl = this.linkGenerator.GetPathByAction("Index", "ResetPassword", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name }),
                RegisterLabel = this.accountLocalizer.GetString("Register"),
                RegisterButtonText = this.accountLocalizer.GetString("RegisterButton"),
                RegisterUrl = $"{this.options.Value.BuyerUrl}{AccountsConstants.ApplicationEndpoint}",
                ContactText = this.accountLocalizer.GetString("RegisterContact"),
                DevelopersEmail = componentModel.DevelopersEmail,
                ErrorMessage = componentModel.ErrorMessage
            };

            return viewModel;
        }
    }
}
