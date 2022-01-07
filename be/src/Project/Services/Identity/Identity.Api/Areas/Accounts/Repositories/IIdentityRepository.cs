using Identity.Api.Areas.Accounts.Models;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Repositories
{
    public interface IIdentityRepository
    {
        Task<Guid> SetPassword(Guid? expirationId, string password, string token, string language);
        Task<User> GetUserAsync(Guid? id, string token, string language);
        Task<Guid> SaveAsync(string id, string password, string firstName, string lastName, string token, string language);
    }
}
