using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.DiscountCodes
{
    public interface IDiscountCodesRepository
    {
        /// <summary>Creates a code, or updates it when <paramref name="id"/> is set. Returns the code's id and its new version.</summary>
        Task<(Guid Id, string Version)> SaveAsync(string token, string language, Guid? id, string version, string code, string description, bool isDisabled, IEnumerable<Guid> clientIds);
        Task<PagedResults<IEnumerable<DiscountCode>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<DiscountCode> GetAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id, string version);

        /// <summary>
        /// All the clients the seller can assign a code to, in a stable order. A failed, malformed or incomplete
        /// answer throws, so an unavailable list can never be mistaken for "no clients".
        /// </summary>
        Task<IEnumerable<DiscountCodeClientOption>> GetClientOptionsAsync(string token, string language);
    }
}
