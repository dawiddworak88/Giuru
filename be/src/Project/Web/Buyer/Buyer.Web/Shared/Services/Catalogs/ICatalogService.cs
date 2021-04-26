using Buyer.Web.Shared.DomainModels.Categories;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Catalogs
{
    public interface ICatalogService
    {
        Task<PagedResults<IEnumerable<CatalogItemViewModel>>> GetCatalogProductsAsync(
            string token,
            string language,
            Guid? sellerId,
            bool? hasPrimaryProduct,
            bool? isNew,
            string searchTerm,
            int pageIndex,
            int itemsPerPage);

        Task<IEnumerable<CatalogCategory>> GetCatalogCategoriesAsync(string language, int pageIndex, int itemsPerPage, string orderBy);
    }
}
