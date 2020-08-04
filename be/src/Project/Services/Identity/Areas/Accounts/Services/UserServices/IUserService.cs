using Foundation.Database.Areas.Accounts.Entities;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Services.UserServices
{
    public interface IUserService
    {
        Task<bool> SignInAsync(HttpContext httpContext, string email, string password, string returnUrl);
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<ApplicationUser> ValidateAsync(string email, string password);
    }
}
