using Buyer.Web.Shared.DomainModels.User;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Identity
{
    public interface IIdentityRepository
    {
        Task<User> GetAsync(string token, string language, string email);
    }
}
