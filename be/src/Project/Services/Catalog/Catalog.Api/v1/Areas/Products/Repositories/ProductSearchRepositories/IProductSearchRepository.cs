using Catalog.Api.v1.Areas.Products.SearchModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Products.Repositories.ProductSearchRepositories
{
    public interface IProductSearchRepository
    {
        Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, Guid? categoryId, Guid? sellerId, string searchTerm, int pageIndex, int itemsPerPage);
        Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, IEnumerable<Guid> ids);
        Task<PagedResults<IEnumerable<ProductSearchModel>>> GetProductVariantsAsync(Guid id, string language);
        Task<ProductSearchModel> GetAsync(Guid id, string language);
        Task<IEnumerable<string>> GetProductSuggestionsAsync(string searchTerm, int size, string language);
    }
}
