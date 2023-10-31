using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.DeliveryAddresses
{
    public interface IClientDeliveryAddresses
    {
        Task<Guid> SaveAsync(string token, string language, string company, string firstName, string lastName, string phoneNumber, string street, string region, string postCode, Guid? clientId, Guid? countryId);
        Task<ClientDeliveryAddress> GetAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<ClientDeliveryAddress>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
    }
}
