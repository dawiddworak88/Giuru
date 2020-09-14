using Catalog.Api.v1.Areas.Products.SearchResultModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Products.Repositories.ProductSearchRepositories
{
    public interface IProductSearchRepository
    {
        Task<PagedResults<IEnumerable<ProductSearchResultModel>>> GetAsync(string language, Guid? categoryId, Guid? brandId, string searchTerm, int pageIndex, int itemsPerPage);
    }
}
