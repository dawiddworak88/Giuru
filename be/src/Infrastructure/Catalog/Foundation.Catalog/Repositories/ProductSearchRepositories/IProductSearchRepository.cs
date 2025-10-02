using Foundation.Catalog.SearchModels.Products;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foundation.Catalog.Repositories.ProductSearchRepositories
{
    public interface IProductSearchRepository
    {
        Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(
            string language, 
            Guid? categoryId, 
            Guid? sellerId, 
            bool? hasPrimaryProduct,
            bool? isNew,
            bool? isSeller,
            string searchTerm,
            int? pageIndex, 
            int? itemsPerPage,
            string orderBy);
        Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, Guid? organisationId, bool? isSeller, IEnumerable<Guid> ids, string orderBy);
        Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, Guid? organisationId, bool? isSeller, IEnumerable<string> skus, string orderBy);
        Task<PagedResults<IEnumerable<ProductSearchModel>>> GetProductVariantsAsync(Guid id, string language, Guid? organisationId, bool? isSeller);
        Task<ProductSearchModel> GetByIdAsync(Guid id, string language, Guid? organisationId, bool? isSeller);
        Task<ProductSearchModel> GetBySkuAsync(string sku, string language, Guid? organisationId, bool? isSeller);
        Task<int?> CountAllAsync();
        IEnumerable<string> GetProductSuggestions(string searchTerm, int size, string language, Guid? organisationId);
    }
}
