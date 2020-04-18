using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Feature.Account.Services.UserServices
{
    public interface IUserService
    {
        Task<bool> SignInAsync(HttpContext httpContext, string email, string password, string returnUrl);
    }
}
