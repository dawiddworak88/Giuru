using System;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Repositories.Identity
{
    public interface IIdentityRepository
    {
        Task<Guid> SaveAsync(string token, string language, string name, string email, string communicationLanguage, string returnUrl);
        Task<Guid> UpdateAsync(string token, string language, Guid? id, string email, string name, string communicationLanguage);
    }
}
