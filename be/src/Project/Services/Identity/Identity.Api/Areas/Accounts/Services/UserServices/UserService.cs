using Feature.Account;
using Foundation.Account.Definitions;
using Foundation.Extensions.Exceptions;
using Identity.Api.Configurations;
using Identity.Api.Infrastructure.Accounts.Entities;
using Identity.Api.Services.Organisations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOrganisationService _organisationService;
        private readonly IOptions<AppSettings> _options;
        private readonly IStringLocalizer<AccountResources> _accountLocalizer;

        public UserService(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IOrganisationService organisationService,
            IStringLocalizer<AccountResources> accountLocalizer,
            IOptions<AppSettings> options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _organisationService = organisationService;
            _options = options;
        }

        public string GeneratePasswordHash(ApplicationUser user, string password)
        {
            return _userManager.PasswordHasher.HashPassword(user, password);
        }

        public async Task SignInAsync(string email, string password, string redirectUrl, string clientId)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is not null)
            {
                if (user.EmailConfirmed is false)
                {
                    throw new CustomException(_accountLocalizer.GetString("ConfirmEmail"), (int)HttpStatusCode.BadRequest);
                }

                if (user.IsActive is false)
                {
                    throw new CustomException(_accountLocalizer.GetString("AccountIsInactive"), (int)HttpStatusCode.BadRequest);
                }

                if (clientId == _options.Value.SellerClientId.ToString() && !await _organisationService.IsSellerAsync(user.OrganisationId))
                {
                    throw new CustomException(_accountLocalizer.GetString("AccessDenied"), (int)HttpStatusCode.Unauthorized);
                }

                if (await _userManager.CheckPasswordAsync(user, password) is false)
                {
                    throw new CustomException(_accountLocalizer.GetString("IncorrectEmailOrPassword"), (int)(HttpStatusCode.OK));
                }

                var properties = new AuthenticationProperties
                {
                    AllowRefresh = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(AccountConstants.TokenLifetimes.DefaultTokenLifetimeInDays),
                    IsPersistent = true,
                    RedirectUri = redirectUrl,
                    IssuedUtc = DateTime.UtcNow
                };

                await _signInManager.SignInAsync(user, properties);
            }
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
