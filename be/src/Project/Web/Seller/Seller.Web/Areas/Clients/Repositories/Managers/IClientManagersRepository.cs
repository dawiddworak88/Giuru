using System;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.Managers
{
    public interface IClientManagersRepository
    {
        Task<Guid> SaveAsync(string token, string language, Guid? id, string firstName, string lastName, string email, string phoneNumber);
    }
}
