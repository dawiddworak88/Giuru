using Identity.Api.Areas.Accounts.Models;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Repositories
{
    public interface IClientRepository
    {
        public Task<Guid?> SaveMarketingApprovals(string token, string language, Client client);
        public Task<Client> GetClientByOrganistationId(string language, string token, Guid? id);
    }
}
