using Foundation.Account.Definitions;
using Identity.Api.Infrastructure.Accounts.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public string GeneratePasswordHash(ApplicationUser user, string password)
        {
            return this.userManager.PasswordHasher.HashPassword(user, password);
        }

        public async Task<bool> SignInAsync(string email, string password, string redirectUrl)
        {
            var user = await this.userManager.FindByEmailAsync(email);

            if (user != null)
            {
                if (await this.userManager.CheckPasswordAsync(user, password))
                {
                    var properties = new AuthenticationProperties
                    {
                        AllowRefresh = false,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(AccountConstants.DefaultTokenLifetimeInDays),
                        IsPersistent = true,
                        RedirectUri = redirectUrl
                    };

                    await this.signInManager.SignInAsync(user, properties);

                    return true;
                }
            }

            return false;
        }

        public async Task SignOutAsync()
        {
            await this.signInManager.SignOutAsync();
        }
    }
}
