using Buyer.Web.Shared.DomainModels.Clients;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Clients
{
    public interface IClientsRepository
    {
        Task<Client> GetClientAsync(string token, string language);
        Task CreateClientApplicationAsync(
            string token, string language, string firstName, string lastName, string contactJobTitle, string email, string phoneNumber,
            string companyName, string companyAddress, string companyCountry, string companyCity, string companyRegion, string companyPostalCode);
    }
}
