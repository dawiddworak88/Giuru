using Feature.Account;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.Definitions;
using Identity.Api.Areas.Accounts.ViewModels;
using Identity.Api.Services.Tokens;
using Identity.Api.Services.Users;
using Identity.Api.ServicesModels.Users;
using Identity.Api.Configurations;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Threading.Tasks;
using Identity.Api.Areas.Accounts.Models;
using Identity.Api.Areas.Accounts.Repositories.ClientNotificationTypes;

namespace Identity.Api.Areas.Accounts.ModelBuilders
{
    public class SetPasswordFormModelBuilder : IAsyncComponentModelBuilder<SetPasswordFormComponentModel, SetPasswordFormViewModel>
    {
        private readonly IUsersService _usersService;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<AccountResources> _accountLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IClientNotificationTypesRepository _clientNotificationTypeRepository;
        private readonly ITokenService _tokenService;
        private readonly IOptions<AppSettings> _options;

        public SetPasswordFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<AccountResources> accountLocalizer, 
            LinkGenerator linkGenerator,
            IUsersService usersService,
            IClientNotificationTypesRepository clientNotificationTypeRepository,
            ITokenService tokenService,
            IOptions<AppSettings> options)
        {
            _globalLocalizer = globalLocalizer;
            _accountLocalizer = accountLocalizer;
            _linkGenerator = linkGenerator;
            _usersService = usersService;
            _clientNotificationTypeRepository = clientNotificationTypeRepository;
            _tokenService = tokenService;
            _options = options;
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
                MarketingApprovalText = _accountLocalizer.GetString("MarketingApprovalText")
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

            var token = await _tokenService.GetTokenAsync(_options.Value.ApiEmail, _options.Value.ApiOrganisationId, _options.Value.ApiAppSecret);

            var notificationTypes = await _clientNotificationTypeRepository.GetByIds(token, componentModel.Language, $"{ClientNotificationTypeConstants.SmsMarketingApprovalId},{ClientNotificationTypeConstants.EmailMarketingApprovalId}", null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(ClientNotificationTypeApproval.CreatedDate)} desc");

            if (notificationTypes is not null)
            {
                viewModel.NotificationTypes = notificationTypes;
            }
            
            return viewModel;
        }
    }
}
