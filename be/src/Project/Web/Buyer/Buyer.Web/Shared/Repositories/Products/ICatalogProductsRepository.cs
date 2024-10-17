using Buyer.Web.Shared.DomainModels.CatalogProducts;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Products
{
    public interface ICatalogProductsRepository
    {
        Task<PagedResults<IEnumerable<CatalogProduct>>> GetProductsAsync(
            string token,
            string language,
            string searchTerm,
            bool? hasPrimaryProduct,
            bool? isNew,
            Guid? sellerId,
            int pageIndex,
            int itemsPerPage,
            string orderBy);

        Task<IEnumerable<CatalogProduct>> GetProductsAsync(string token, string language, string skus);
    }
}
