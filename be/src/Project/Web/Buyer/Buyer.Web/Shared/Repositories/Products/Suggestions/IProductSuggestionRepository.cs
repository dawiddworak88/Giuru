using Buyer.Web.Areas.Products.DomainModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Products.Suggestions
{
    public interface IProductSuggestionRepository
    {
        Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(string token, string language, string searchTerm, bool? hasPrimaryProduct, Guid? sellerId, int pageIndex, int itemsPerPage, string orderBy);
    }
}
