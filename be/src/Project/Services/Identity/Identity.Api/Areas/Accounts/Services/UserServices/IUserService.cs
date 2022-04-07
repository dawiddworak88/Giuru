using Identity.Api.Infrastructure.Accounts.Entities;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Services.UserServices
{
    public interface IUserService
    {
        string GeneratePasswordHash(ApplicationUser user, string password);
        Task<bool> SignInAsync(string email, string password, string redirectUrl, string clientId);
        Task SignOutAsync();
    }
}
