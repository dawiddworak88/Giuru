using Identity.Api.ServicesModels.Roles;
using System.Threading.Tasks;

namespace Identity.Api.Services.Roles
{
    public interface IRolesService
    {
        Task AssignRolesAsync(CreateRolesServiceModel model);
    }
}
