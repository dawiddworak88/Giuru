using Identity.Api.ServicesModels.Users;
using System.Threading.Tasks;

namespace Identity.Api.Services.Users
{
    public interface IUsersService
    {
        Task<UserServiceModel> CreateAsync(CreateUserServiceModel serviceModel);
        Task<UserServiceModel> GetById(GetUserServiceModel serviceModel);
        Task<UserServiceModel> GetByExpirationId(GetUserServiceModel serviceModel);
        Task<UserServiceModel> UpdateAsync(UpdateUserServiceModel serviceModel);
        Task<UserServiceModel> SetPasswordAsync(SetUserPasswordServiceModel serviceModel);
        Task ResetPasswordAsync(ResetUserPasswordServiceModel serviceModel);
        Task<UserServiceModel> GetByEmail(GetUserByEmailServiceModel serviceModel);
    }
}
