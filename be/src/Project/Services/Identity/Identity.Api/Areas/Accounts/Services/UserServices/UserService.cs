using Foundation.Account.Definitions;
using Foundation.Extensions.Exceptions;
using Identity.Api.Configurations;
using Identity.Api.Infrastructure.Accounts.Entities;
using Identity.Api.Services.Organisations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IOrganisationService organisationService;
        private readonly IOptions<AppSettings> options;

        public UserService(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IOrganisationService organisationService,
            IOptions<AppSettings> options)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.organisationService = organisationService;
            this.options = options;
        }

        public string GeneratePasswordHash(ApplicationUser user, string password)
        {
            return this.userManager.PasswordHasher.HashPassword(user, password);
        }

        public async Task SignInAsync(string email, string password, string redirectUrl, string clientId)
        {
            var user = await this.userManager.FindByEmailAsync(email);

            if (user is not null)
            {
                if (user.EmailConfirmed is false)
                {
                    throw new CustomException("Potwierdz adres email", (int)HttpStatusCode.BadRequest);
                }

                if (user.IsActive is false)
                {
                    throw new CustomException("Konto zostało dezaktywowane. Jeśli chcesz je ponownie aktywować, to skontaktuj się ze swoim opiekunem", (int)HttpStatusCode.BadRequest);
                }

                if (clientId == this.options.Value.SellerClientId.ToString() && !await this.organisationService.IsSellerAsync(user.OrganisationId))
                {
                    throw new CustomException("Brak dostępu", (int)HttpStatusCode.OK);
                }

                if (await this.userManager.CheckPasswordAsync(user, password) is false)
                {
                    throw new CustomException("IncorrectEmailOrPassword", (int)(HttpStatusCode.OK));
                }

                var properties = new AuthenticationProperties
                {
                    AllowRefresh = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(AccountConstants.TokenLifetimes.DefaultTokenLifetimeInDays),
                    IsPersistent = true,
                    RedirectUri = redirectUrl,
                    IssuedUtc = DateTime.UtcNow
                };

                await this.signInManager.SignInAsync(user, properties);
            }
        }

        public async Task SignOutAsync()
        {
            await this.signInManager.SignOutAsync();
        }
    }
}
