using Identity.Api.ServicesModels.Users;
using System.Threading.Tasks;

namespace Identity.Api.Services.Users
{
    public interface IUsersService
    {
        Task<UserServiceModel> CreateAsync(CreateUserServiceModel serviceModel);
        Task<UserServiceModel> GetAsync(GetUserServiceModel serviceModel);
        Task<UserServiceModel> UpdateAsync(UpdateUserServiceModel serviceModel);
        Task<UserServiceModel> SetPasswordAsync(SetUserPasswordServiceModel serviceModel);
    }
}
