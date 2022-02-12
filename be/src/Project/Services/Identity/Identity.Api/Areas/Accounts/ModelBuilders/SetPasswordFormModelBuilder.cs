using Feature.Account;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.ViewModels;
using Identity.Api.Services.Users;
using Identity.Api.ServicesModels.Users;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ModelBuilders
{
    public class SetPasswordFormModelBuilder : IAsyncComponentModelBuilder<SetPasswordFormComponentModel, SetPasswordFormViewModel>
    {
        private readonly IUsersService usersService;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<AccountResources> accountLocalizer;
        private readonly LinkGenerator linkGenerator;

        public SetPasswordFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<AccountResources> accountLocalizer, 
            LinkGenerator linkGenerator,
            IUsersService usersService)
        {
            this.globalLocalizer = globalLocalizer;
            this.accountLocalizer = accountLocalizer;
            this.linkGenerator = linkGenerator;
            this.usersService = usersService;
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
                EmailIsConfirmedText = this.accountLocalizer["EmailIsConfirmedText"],
                BackToLoginText = this.accountLocalizer["BackToLoginText"],
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                PasswordSetSuccessMessage = this.accountLocalizer.GetString("PasswordUpdated")
            };

            if (componentModel.Id.HasValue)
            {
                var serviceModel = new GetUserServiceModel
                {
                    Id = componentModel.Id.Value,
                    Language = componentModel.Language
                };

                var user = await this.usersService.GetByExpirationId(serviceModel);

                if (user is not null)
                {
                    viewModel.Id = componentModel.Id.Value;
                }
            }

            return viewModel;
        }
    }
}
