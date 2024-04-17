using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.Applications
{
    public interface IClientApplicationsRepository
    {
        Task<PagedResults<IEnumerable<ClientApplication>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<ClientApplication> GetAsync(string token, string language, Guid? id);
        Task<Guid> SaveAsync(
            string token, string language, Guid? id, string companyName, string firstName, string lastName, string contactJobTitle,
            string email, string phoneNumber, string communicationLanguage, bool isDeliveryAddressEqualBillingAddress,
            ClientApplicationAddressRequestModel billingAddress, ClientApplicationAddressRequestModel deliveryAddress);
    }
}
