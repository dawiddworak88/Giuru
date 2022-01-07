using Feature.Account;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.Repositories;
using Identity.Api.Areas.Accounts.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ModelBuilders
{
    public class SetPasswordFormModelBuilder : IAsyncComponentModelBuilder<SetPasswordFormComponentModel, SetPasswordFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<AccountResources> accountLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IIdentityRepository identityRepository;

        public SetPasswordFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<AccountResources> accountLocalizer, 
            LinkGenerator linkGenerator,
            IIdentityRepository identityRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.accountLocalizer = accountLocalizer;
            this.linkGenerator = linkGenerator;
            this.identityRepository = identityRepository;
        }

        public async Task<SetPasswordFormViewModel> BuildModelAsync(SetPasswordFormComponentModel componentModel)
        {
            var viewModel = new SetPasswordFormViewModel
            {
                Id = componentModel.Id.Value,
                ReturnUrl = componentModel.ReturnUrl,
                PasswordFormatErrorMessage = this.globalLocalizer["PasswordFormatErrorMessage"],
                PasswordRequiredErrorMessage = this.globalLocalizer["PasswordRequiredErrorMessage"],
                PasswordLabel = this.globalLocalizer["EnterPasswordText"],
                ConfirmPasswordLabel = this.globalLocalizer["EnterConfirmPasswordText"],
                SetPasswordText = this.accountLocalizer["SetPassword"],
                SubmitUrl = this.linkGenerator.GetPathByAction("Index", "IdentityApi", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name }),
                FirstNameRequiredErrorMessage = this.accountLocalizer.GetString("FirstNameRequiredErrorMessage"),
                LastNameRequiredErrorMessage = this.accountLocalizer.GetString("LastNameRequiredErrorMessage"),
            };

            if (componentModel.Id.HasValue)
            {
                var user = await this.identityRepository.GetUserAsync(componentModel.Id, componentModel.Token, componentModel.Language);
                if (user != null)
                {
                    viewModel.Id = componentModel.Id.Value;
                    viewModel.EmailConfirmed = user.EmailConfirmed;
                }
            }

            return viewModel;
        }
    }
}
