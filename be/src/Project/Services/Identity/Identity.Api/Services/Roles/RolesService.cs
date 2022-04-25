using Foundation.Extensions.ExtensionMethods;
using Identity.Api.ServicesModels.Roles;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Identity.Api.Services.Roles
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesService(
            RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task CreateAsync(CreateRolesServiceModel model)
        {
            foreach (var role in model.Roles.OrEmptyIfNull())
            {
                var existingRole = await this.roleManager.RoleExistsAsync(role);

                if (existingRole is false)
                {
                    await this.roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
