using Catalog.Api.v1.Areas.Products.SearchModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Products.Repositories.ProductSearchRepositories
{
    public interface IProductSearchRepository
    {
        Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, Guid? categoryId, Guid? brandId, string searchTerm, int pageIndex, int itemsPerPage, bool primaryProductsOnly, bool productVariantsOnly);
        Task<ProductSearchModel> GetAsync(Guid id, string language);
    }
}
