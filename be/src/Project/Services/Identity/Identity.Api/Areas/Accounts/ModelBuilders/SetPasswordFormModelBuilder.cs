using Feature.Account;
using Foundation.Extensions.ModelBuilders;
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
using System.Linq;
using Identity.Api.Services.Approvals;
using Identity.Api.ServicesModels.Approvals;
using Foundation.GenericRepository.Paginations;
using Foundation.Security.Definitions;
using Identity.Api.Repositories.GraphQl;

namespace Identity.Api.Areas.Accounts.ModelBuilders
{
    public class SetPasswordFormModelBuilder : IAsyncComponentModelBuilder<SetPasswordFormComponentModel, SetPasswordFormViewModel>
    {
        private readonly IUsersService _usersService;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<AccountResources> _accountLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly ITokenService _tokenService;
        private readonly IOptions<AppSettings> _options;
        private readonly IApprovalsService _approvalsService;
        private readonly IGraphQlRepository _graphQlRepository; 

        public SetPasswordFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<AccountResources> accountLocalizer, 
            LinkGenerator linkGenerator,
            IUsersService usersService,
            ITokenService tokenService,
            IOptions<AppSettings> options,
            IApprovalsService approvalsService,
            IGraphQlRepository graphQlRepository)
        {
            _globalLocalizer = globalLocalizer;
            _accountLocalizer = accountLocalizer;
            _linkGenerator = linkGenerator;
            _usersService = usersService;
            _tokenService = tokenService;
            _options = options;
            _approvalsService = approvalsService;
            _graphQlRepository = graphQlRepository;
        }

        public async Task<SetPasswordFormViewModel> BuildModelAsync(SetPasswordFormComponentModel componentModel)
        {
            var viewModel = new SetPasswordFormViewModel
            {
                Id = componentModel.Id,
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
                PersonalDataAdministratorText = _globalLocalizer.GetString("PersonalDataAdministrator")
            };

            viewModel.PersonalDataAdministratorText = await _graphQlRepository.GetTextAsync(componentModel.Language, "en", GraphQlConstants.PersonalDataAdministrator);
            viewModel.MarketingApprovalText = await _graphQlRepository.GetTextAsync(componentModel.Language, "en", GraphQlConstants.MarketingFormDescription);

            var getApprovalsServiceModel = new GetApprovalsServiceModel
            {
                SearchTerm = null,
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize,
                OrderBy = null,
                Language = CultureInfo.CurrentUICulture.Name,
            };

            var approvals = _approvalsService.Get(getApprovalsServiceModel);

            viewModel.Approvals = approvals.Data.Where(x => x.Id == ApprovalsConstants.Marketing.InformationByEmail || x.Id == ApprovalsConstants.Marketing.InformationBySms)
                .Select(x => new ApprovalViewModel 
                { 
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                });

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
            
            return viewModel;
        }
    }
}
