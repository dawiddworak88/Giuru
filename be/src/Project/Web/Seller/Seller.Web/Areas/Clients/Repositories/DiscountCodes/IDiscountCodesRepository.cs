using Foundation.GenericRepository.Paginations;
using Foundation.Pricing.DiscountCodes;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.DiscountCodes
{
    public interface IDiscountCodesRepository
    {
        Task<Guid> SaveAsync(string token, string language, Guid? id, string code, string description, bool isDisabled);
        Task<PagedResults<IEnumerable<DiscountCode>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<IEnumerable<DiscountCode>> GetAsync(string token, string language);
        Task<DiscountCode> GetAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);

        /// <summary>
        /// Asks Client.Api whether a code can be applied for a client. Throws when the call fails: a failed call is not
        /// an answer, so the caller must not read it as one.
        /// </summary>
        Task<DiscountCodeValidation> ValidateAsync(string token, string language, Guid clientId, string code, CancellationToken cancellationToken = default);
    }
}
