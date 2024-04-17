using Buyer.Web.Shared.ApiRequestModels.Application;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Clients.Repositories
{
    public interface IApplicationsRepository
    {
        Task CreateClientApplicationAsync(
            string token, string language, string companyName, string firstName, string lastName,
            string contactJobTitle, string email, string phoneNumber, string communicationLanguage,
            bool isDeliveryAddressEqualBillingAddress,
            ClientApplicationAddressRequestModel billingAddress, ClientApplicationAddressRequestModel deliveryAddress);
    }
}
