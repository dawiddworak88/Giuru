using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.ViewModels.SignInForm;
using Feature.Account;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Identity.Api.ModelBuilders.SignInForm
{
    public class SignInFormModelBuilder : IComponentModelBuilder<SignInFormComponentModel, SignInFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<AccountResources> accountLocalizer;
        private readonly LinkGenerator linkGenerator;

        public SignInFormModelBuilder(IStringLocalizer<GlobalResources> globalLocalizer, IStringLocalizer<AccountResources> accountLocalizer, LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.accountLocalizer = accountLocalizer;
            this.linkGenerator = linkGenerator;
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
                ReturnUrl = componentModel.ReturnUrl
            };

            return viewModel;
        }
    }
}
