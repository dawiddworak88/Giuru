using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Repositories.Clients
{
    public interface IClientsRepository
    {
        Task CreateClientApplicationAsync(
            string token, string language, string firstName, string lastName, string contactJobTitle, string email, string phoneNumber,
            string companyName, string companyAddress, string companyCountry, string companyCity, string companyRegion, string companyPostalCode);
    }
}
