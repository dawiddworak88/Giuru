using Identity.Api.Areas.Accounts.Models;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Repositories
{
    public interface IClientRepository
    {
        public Task<Guid?> SaveAsync(string token, string language, Client client);
        public Task<Client> GetByOrganisationAsync(string language, string token, Guid? id);
    }
}
