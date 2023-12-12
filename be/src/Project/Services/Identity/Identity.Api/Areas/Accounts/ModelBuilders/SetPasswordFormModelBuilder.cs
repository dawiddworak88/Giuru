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
        private readonly IUsersService _usersService;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<AccountResources> _accountLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public SetPasswordFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<AccountResources> accountLocalizer, 
            LinkGenerator linkGenerator,
            IUsersService usersService)
        {
            _globalLocalizer = globalLocalizer;
            _accountLocalizer = accountLocalizer;
            _linkGenerator = linkGenerator;
            _usersService = usersService;
        }

        public async Task<SetPasswordFormViewModel> BuildModelAsync(SetPasswordFormComponentModel componentModel)
        {
            var viewModel = new SetPasswordFormViewModel
            {
                Id = componentModel.Id.Value,
                ReturnUrl = componentModel.ReturnUrl,
                PasswordFormatErrorMessage = _globalLocalizer["PasswordFormatErrorMessage"],
                PasswordRequiredErrorMessage = _globalLocalizer["PasswordRequiredErrorMessage"],
                PasswordLabel = _globalLocalizer["EnterPasswordText"],
                ConfirmPasswordLabel = _globalLocalizer["EnterConfirmPasswordText"],
                SetPasswordText = _accountLocalizer["SetPassword"],
                SubmitUrl = _linkGenerator.GetPathByAction("Index", "IdentityApi", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name }),
                EmailIsConfirmedText = _accountLocalizer["EmailIsConfirmedText"],
                BackToLoginText = _accountLocalizer["BackToLoginText"],
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                PasswordSetSuccessMessage = _accountLocalizer.GetString("PasswordUpdated"),
                MarketingApprovalHeader = _accountLocalizer.GetString("MarketingApprovalHeader"),
                MarketingApprovalText = _accountLocalizer.GetString("MarketingApprovalText"),
                EmailMarketingApprovalLabel = _accountLocalizer.GetString("EmailMarketingApprovalLabel"),
                SmsMarketingApprovalLabel = _accountLocalizer.GetString("SmsMarketingApprovalLabel")
            };

            if (componentModel.Id.HasValue)
            {
                var serviceModel = new GetUserServiceModel
                {
                    Id = componentModel.Id.Value,
                    Language = componentModel.Language
                };

                var user = await _usersService.GetByExpirationId(serviceModel);

                if (user is not null)
                {
                    viewModel.Id = componentModel.Id.Value;
                }
            }

            return viewModel;
        }
    }
}
