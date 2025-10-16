using Feature.Account;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Identity.Api.Infrastructure.Accounts.Entities;
using Identity.Api.ServicesModels.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.Services.Roles
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStringLocalizer<AccountResources> accountLocalizer;

        public RolesService(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<AccountResources> accountLocalizer)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.accountLocalizer = accountLocalizer;
        }

        public async Task AssignRolesAsync(CreateRolesServiceModel model)
        {
            var user = await this.userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                throw new NotFoundException(this.accountLocalizer.GetString("UserNotFound"));
            }

            var userRoles = await this.userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles.OrEmptyIfNull())
            {
                await this.userManager.RemoveFromRoleAsync(user, userRole);
            }

            foreach (var role in model.Roles.OrEmptyIfNull())
            {
  
                var roles = await this.roleManager.RoleExistsAsync(role);

                if (roles is false)
                {
                    await this.roleManager.CreateAsync(new IdentityRole(role));
                }

                await this.userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
