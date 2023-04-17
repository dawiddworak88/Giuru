using Analytics.Api.DomainModels;
using System;
using System.Threading.Tasks;

namespace Analytics.Api.Repositories.Clients
{
    public interface IClientRepository
    {
        Task<Client> GetAsync(string token, Guid? id);
    }
}
